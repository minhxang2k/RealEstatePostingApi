using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Context;
namespace RealEstatePostingApi.Services
{
    public class WebsiteTrafficCleanUpService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<WebsiteTrafficCleanUpService> _logger;
        public WebsiteTrafficCleanUpService(IServiceScopeFactory scopeFactory, ILogger<WebsiteTrafficCleanUpService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var cutoffDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-1));
                    var oldRecords = await context.Website_Traffic
                        .Where(wt => wt.Visit_Date < cutoffDate)
                        .ToListAsync();
                    if (oldRecords.Any())
                    {
                        context.Website_Traffic.RemoveRange(oldRecords);
                        await context.SaveChangesAsync();
                        _logger.LogInformation($"Deleted {oldRecords.Count} old website traffic records.");
                    }
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Run once a day
            }
        }
    
    }
}
