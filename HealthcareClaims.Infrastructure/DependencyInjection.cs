using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;
using HealthcareClaims.Infrastructure.Services;

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
        services.AddHttpContextAccessor();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuditService, AuditService>();

        return services;
    }
}