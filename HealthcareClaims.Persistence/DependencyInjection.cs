using Microsoft.Extensions.DependencyInjection;

namespace HealthcareClaims.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // DbContext registration will come next

        return services;
    }
}