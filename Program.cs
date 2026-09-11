using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RealEstatePostingApi.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Entity Framework DbContext
//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var conString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
    throw new InvalidOperationException("Connection string 'DefaultConnection'" 
    +" not found.");

// Add connection string to the DbContext
builder.Services.AddDbContext<AppDbContext>(options => 
options.UseSqlServer(conString)) ;

//heal check middleware
var hc = builder.Services.AddHealthChecks();

//if the application is runing
hc.AddCheck(
    name: "self-live",
    check: () => HealthCheckResult.Healthy("Application is live"),
    tags: new[] { "live" }
    );

//if the application can serve requests
hc.AddCheck(
    name: "self-ready",
    check: () => HealthCheckResult.Healthy("Application is ready to serve"),
    tags: new[] { "ready" }
    );

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpsRedirection();
}

//app.UseHttpsRedirection();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    AllowCachingResponses = false,
    Predicate = r => r.Tags.Contains("live"),
});

app.UseHealthChecks("/ready", new HealthCheckOptions
{
    AllowCachingResponses = false,
    Predicate = r => r.Tags.Contains("ready"),
});

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();

