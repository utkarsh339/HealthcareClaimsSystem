using FluentValidation;
using HealthcareClaims.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HealthcareClaims.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR here later
        // Register FluentValidation later

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                typeof(DependencyInjection).Assembly);
        });

        services.AddValidatorsFromAssembly(
    typeof(DependencyInjection).Assembly);

        services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

        return services;
    }
}