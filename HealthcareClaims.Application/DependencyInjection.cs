using Microsoft.Extensions.DependencyInjection;

namespace HealthcareClaims.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR here later
        // Register FluentValidation later

        return services;
    }
}