using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RealEstatePostingApi.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


//add jwt authentication
var jwtSettings = builder.Configuration.GetSection("JWT");

var key = jwtSettings["Key"];
var Issuer = jwtSettings["Issuer"];
var Audience = jwtSettings["Audience"];
var ExpireMinutes = jwtSettings["ExpiresDays"];

var jwtAuthenticationBuilder = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key!)
            )
        };
    });

if(jwtAuthenticationBuilder == null)
{
    throw new InvalidOperationException("Failed to configure JWT authentication.");
}

// Add Entity Framework DbContext
var conString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection'"
    + " not found.");

//var conString = builder.Configuration.GetConnectionString("DefaultConnection")
//    ?? throw new InvalidOperationException(
//        "Connection string 'DefaultConnection' not found."
//    );

//Console.WriteLine("==================================");
//Console.WriteLine($"DB Connection: {conString}");
//Console.WriteLine("==================================");

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

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();

