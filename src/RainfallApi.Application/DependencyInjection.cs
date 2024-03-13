using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RainfallApi.Application.Common.Behaviors;

namespace RainfallApi.Application;

/// <summary>
/// Extension method to configure application services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds application services to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            // Registers all MediatR handlers and behaviors from the current assembly.
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            // Adds the custom validation behavior for MediatR requests.
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Registers validators from the assembly containing the DependencyInjection class.
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return services;
    }
}
