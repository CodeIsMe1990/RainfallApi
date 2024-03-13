using Microsoft.AspNetCore.Builder;

namespace RainfallApi.Infrastructure;

public static class RequestPipeline
{
    /// <summary>
    /// Extension method to configure infrastructure-related middleware in the request pipeline.
    /// </summary>
    /// <param name="app">An <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        // Placeholder for adding infrastructure-related middleware or configuring the request pipeline.
        // Currently, this method does not perform any operations and simply returns the provided app instance.
        return app;
    }
}
