using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace HealthcareClaims.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string 'DefaultConnection' is not configured.");
        }

        connectionString = NormalizePostgresConnectionString(connectionString);

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
            }));

        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }

    private static string NormalizePostgresConnectionString(string connectionString)
    {
        // Render provides PostgreSQL URLs in this format:
        // postgresql://username:password@host:port/database
        //
        // Npgsql expects a key/value connection string:
        // Host=...;Port=...;Database=...;Username=...;Password=...

        if (!connectionString.StartsWith(
                "postgresql://",
                StringComparison.OrdinalIgnoreCase) &&
            !connectionString.StartsWith(
                "postgres://",
                StringComparison.OrdinalIgnoreCase))
        {
            // Local development connection strings are already in
            // Npgsql's expected format.
            return connectionString;
        }

        var uri = new Uri(connectionString);

        var userInfo = uri.UserInfo.Split(':', 2);

        if (userInfo.Length != 2)
        {
            throw new InvalidOperationException(
                "Invalid PostgreSQL connection URL. Username and password are required.");
        }

        var username = Uri.UnescapeDataString(userInfo[0]);
        var password = Uri.UnescapeDataString(userInfo[1]);
        var database = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/'));

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Database = database,
            Username = username,
            Password = password
        };

        return builder.ConnectionString;
    }
}