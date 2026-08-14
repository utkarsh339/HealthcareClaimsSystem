# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["HealthcareClaims.API/HealthcareClaims.API.csproj", "HealthcareClaims.API/"]
COPY ["HealthcareClaims.Application/HealthcareClaims.Application.csproj", "HealthcareClaims.Application/"]
COPY ["HealthcareClaims.Domain/HealthcareClaims.Domain.csproj", "HealthcareClaims.Domain/"]
COPY ["HealthcareClaims.Infrastructure/HealthcareClaims.Infrastructure.csproj", "HealthcareClaims.Infrastructure/"]
COPY ["HealthcareClaims.Persistence/HealthcareClaims.Persistence.csproj", "HealthcareClaims.Persistence/"]

RUN dotnet restore "HealthcareClaims.API/HealthcareClaims.API.csproj"

COPY . .
WORKDIR "/src/HealthcareClaims.API"
RUN dotnet publish "HealthcareClaims.API.csproj" -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Render/Linux containers can have a low inotify limit.
# Use polling instead of FileSystemWatcher for file change detection.
ENV DOTNET_USE_POLLING_FILE_WATCHER=1

# ASP.NET Core listens on port 8080 inside the container.
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "HealthcareClaims.API.dll"]