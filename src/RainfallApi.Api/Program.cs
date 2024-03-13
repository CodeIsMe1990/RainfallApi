using RainfallApi.Api;
using RainfallApi.Api.Common.Middleware;
using RainfallApi.Application;
using RainfallApi.Infrastructure;

// Create a new WebApplication builder
var builder = WebApplication.CreateBuilder(args);

// Configure services
{
    builder.Services
        .AddPresentation() // Add presentation-related services
        .AddApplication() // Add application services
        .AddInfrastructure(builder.Configuration);    // Add infrastructure services with configuration
}

// Build the application
var app = builder.Build();

// Configure middleware and pipeline
{
    app.UseExceptionHandler();      // Handle exceptions globally
    app.UseInfrastructure();       // Use infrastructure middleware

    // Use Swagger and Swagger UI in development environment
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ErrorHandlingMiddleware>();   // Custom error handling middleware
    app.UseHttpsRedirection();      // Redirect HTTP requests to HTTPS
    app.UseAuthorization();         // Enable authorization
    app.MapControllers();           // Map controllers

    app.Run();  // Execute the application
}