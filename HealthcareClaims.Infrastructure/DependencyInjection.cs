using Microsoft.Extensions.DependencyInjection;

namespace HealthcareClaims.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Email Service
        // SMS Service
        // File Storage
        // Azure Blob
        // JWT
        // External APIs

        return services;
    }
}