using Microsoft.EntityFrameworkCore;
using RealEstatePostingApi.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Entity Framework DbContext
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        bool canConnect = db.Database.CanConnect();
       
        Console.WriteLine(canConnect ? "✅ Kết nối database thành công!" : "❌ Không kết nối được database.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Lỗi kết nối: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"❌ Lỗi chi tiết: {ex.InnerException.Message}");
        }
    }
}
app.Run();

