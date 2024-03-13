using System.Reflection;

using Microsoft.OpenApi.Models;

namespace RainfallApi.Api;

/// <summary>
/// Static class containing extension methods for IServiceCollection to configure presentation-related services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds presentation-related services to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // Add controllers and endpoints for API controllers
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // Configure Swagger generation options
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Rainfall Api",
                Description = "An API which provides rainfall reading data",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Sorted",
                    Url = new Uri("https://www.sorted.com"),
                },
            });

            // Include XML comments for Swagger documentation
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        // Add support for ProblemDetails
        services.AddProblemDetails();

        return services;
    }
}