# RealEstatePostingApi

A .NET 10 Web API for managing real-estate postings (authentication, EF Core persistence, JWT auth, file uploads). Includes EF Core tooling and Docker support.

## Features
- JWT authentication
- Identity + EF Core (SQL Server)
- OpenAPI (Swagger)
- File uploads (wwwroot/Uploads)
- Docker-ready

## Prerequisites
- .NET 10 SDK
- SQL Server (local or remote) or use a Docker SQL image
- Optional: Docker
- Optional: dotnet-ef tool

## Quick setup (PowerShell)

1. Clone and open repository

   git clone https://github.com/Huynguyen135/RealEstatePostingApi
   cd RealEstatePostingApi

2. Restore and build

   dotnet restore
   dotnet build

3. Configure secrets / environment variables (example using User Secrets)

   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=RealEstateDb;Trusted_Connection=True;"
   dotnet user-secrets set "Jwt:Key" "<your-strong-secret-key>"
   dotnet user-secrets set "Jwt:Issuer" "<issuer>"
   dotnet user-secrets set "Jwt:Audience" "<audience>"

   Or set environment variables:
   $env:ConnectionStrings__DefaultConnection = "..."
   $env:Jwt__Key = "..."

4. Install EF tooling (if needed) and apply migrations

   dotnet tool install --global dotnet-ef
   dotnet ef database update

5. Run the API

   dotnet run

   Or open solution in Visual Studio and run (F5).

## Docker (optional)

Build image:

   docker build -t realestatepostingapi .

Run container (example using env file):

   docker run --env-file .env -p 5000:80 realestatepostingapi

## Package and security commands

- Check vulnerable packages:

  dotnet list package --vulnerable

- Check outdated packages:

  dotnet list package --outdated

- Upgrade or add a package (example shown for a fixed transitive dependency):

  dotnet add package System.Security.Cryptography.Pkcs --version 9.0.21
  dotnet restore

- Upgrade a specific package to latest compatible version:

  dotnet add package <PackageId> --version <Version>
  dotnet restore

## Tests

- Run tests (if present):

  dotnet test

## Useful commands summary

- dotnet restore
- dotnet build
- dotnet run
- dotnet ef database update
- dotnet user-secrets set "Key" "Value"
- dotnet list package --vulnerable
- dotnet list package --outdated
- dotnet add package <PackageId> --version <Version>
- docker build -t realestatepostingapi .
- docker run --env-file .env -p 5000:80 realestatepostingapi

## Troubleshooting

- EF errors: verify connection string and run migrations.
- JWT errors: verify Jwt:Key and Issuer/Audience.
- File upload permission issues: ensure wwwroot/Uploads exists and has write permission.

---

Add or adjust example commands and values to match your environment before running in production.
