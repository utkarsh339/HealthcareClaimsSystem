# Healthcare Claims System — Deployment Guide

## Target architecture

- React/Vite frontend → Netlify
- ASP.NET Core .NET 10 API → Render Docker Web Service
- PostgreSQL → Render PostgreSQL
- Source control → GitHub

## Backend environment variables

Set these in the Render API service:

```text
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_HTTP_PORTS=8080
ConnectionStrings__DefaultConnection=<Render PostgreSQL external/internal connection string>
JwtSettings__SecretKey=<random secret of at least 32 characters>
JwtSettings__Issuer=HealthcareClaimsAPI
JwtSettings__Audience=HealthcareClaimsClient
JwtSettings__ExpiryInMinutes=60
Cors__AllowedOrigins=https://<your-netlify-domain>
Database__EnsureCreated=true
```

Do not commit real database credentials or JWT secrets.

## Database initialization

The project previously contained SQL Server-specific EF Core migrations. Those migrations were removed from the deployable package because they cannot be applied to PostgreSQL safely.

For the initial deployment, `Database:EnsureCreated=true` creates the PostgreSQL schema from the current EF Core model. After the first stable deployment, future schema changes should be converted to PostgreSQL EF Core migrations and applied before deployment rather than continuing to rely on `EnsureCreated`.

## Frontend environment variables

In Netlify, set:

```text
VITE_API_BASE_URL=https://<your-render-api-domain>/api
```

The React app falls back to `/api` only when this variable is absent.

## Local checks

Backend:

```bash
dotnet restore HealthcareClaims.API/HealthcareClaims.API.csproj
dotnet build HealthcareClaimsSystem.slnx -c Release
```

Frontend:

```bash
cd healthcare-claims-frontend
npm ci
npm run build
```

## Render

Create a Docker Web Service from the GitHub repository.

- Dockerfile: `/Dockerfile`
- Port: `8080`
- Health check: `/health`
- Set all backend environment variables listed above.

## Netlify

Create a site from the GitHub repository.

- Base directory: `healthcare-claims-frontend`
- Build command: `npm run build`
- Publish directory: `dist`
- Set `VITE_API_BASE_URL` to the deployed Render API URL.

`netlify.toml` already contains the SPA fallback required by React Router.
