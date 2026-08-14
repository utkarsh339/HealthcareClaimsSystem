# Healthcare Claims System

Production-ready baseline for the Healthcare Claims Processing & Prior Authorization Platform.

## Stack

- Backend: ASP.NET Core .NET 10
- Architecture: Clean Architecture + CQRS/MediatR
- Database: PostgreSQL + EF Core/Npgsql
- Authentication: JWT
- Frontend: React + Vite + Redux Toolkit/RTK Query
- Backend hosting: Render Docker Web Service
- Database hosting: Render PostgreSQL
- Frontend hosting: Netlify

## Deployment

The repository contains:

- `Dockerfile` — production API image
- `render.yaml` — Render API + PostgreSQL Blueprint
- `DEPLOYMENT.md` — deployment instructions and environment variables
- `healthcare-claims-frontend/netlify.toml` — Netlify build and React Router fallback
- `healthcare-claims-frontend/.env.example` — frontend production configuration example
- `Healthcare_Claims_Master_Handbook_v34.txt` — project/deployment status

Start with `DEPLOYMENT.md`.

## Security

Production database credentials and JWT secrets are intentionally not committed. Configure them through Render environment variables.

## Database initialization

The initial PostgreSQL deployment uses EF Core `EnsureCreated` because the previous project migrations were SQL Server provider-specific. Future schema changes should use PostgreSQL EF Core migrations.
