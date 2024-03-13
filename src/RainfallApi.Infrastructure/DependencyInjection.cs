using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RainfallApi.Application.Common.Interfaces;
using RainfallApi.Infrastructure.Services;

namespace RainfallApi.Infrastructure;

/// <summary>
/// Class containing extension methods for dependency injection configuration.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Extension method to add infrastructure-related services to the dependency injection container.
    /// </summary>
    /// <param name="services">An <see cref="IServiceCollection"/> instance.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Adding various infrastructure-related services
        services
            .AddHttpContextAccessor() // Add HTTP context accessor
            .AddServices() // Add services
            .AddBackgroundServices(configuration) // Add background services
            .AddAuthentication(configuration) // Add authentication
            .AddAuthorization() // Add authorization
            .AddPersistence(); // Add persistence

        return services;
    }

    /// <summary>
    /// Extension method to add background services to the dependency injection container.
    /// </summary>
    /// <param name="services">An <see cref="IServiceCollection"/> instance.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    private static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Placeholder for adding background services
        // Additional services can be added here if needed
        return services;
    }

    /// <summary>
    /// Extension method to add services to the dependency injection container.
    /// </summary>
    /// <param name="services">An <see cref="IServiceCollection"/> instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Adding HTTP client and scoped service for RainfallApiService
        services.AddHttpClient();
        services.AddScoped<IRainfallApiService, RainfallApiService>();

        return services;
    }

    /// <summary>
    /// Extension method to add persistence-related services to the dependency injection container.
    /// </summary>
    /// <param name="services">An <see cref="IServiceCollection"/> instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // Placeholder for adding persistence-related services
        // Additional persistence-related services can be added here if needed
        return services;
    }

    /// <summary>
    /// Extension method to add authorization-related services to the dependency injection container.
    /// </summary>
    /// <param name="services">An <see cref="IServiceCollection"/> instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        // Placeholder for adding authorization-related services
        // Additional authorization-related services can be added here if needed
        return services;
    }

    /// <summary>
    /// Extension method to add authentication-related services to the dependency injection container.
    /// </summary>
    /// <param name="services">An <see cref="IServiceCollection"/> instance.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Placeholder for adding authentication-related services
        // Additional authentication-related services can be added here if needed
        return services;
    }
}