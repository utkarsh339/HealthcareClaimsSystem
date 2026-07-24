using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Infrastructure.Authentication;
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
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}