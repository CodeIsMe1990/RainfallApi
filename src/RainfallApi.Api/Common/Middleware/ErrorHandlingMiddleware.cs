using Newtonsoft.Json;

using RainfallApi.Contracts.Common;

namespace RainfallApi.Api.Common.Middleware;

/// <summary>
/// Middleware for handling exceptions and returning appropriate error responses.
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next RequestDelegate in the middleware pipeline.</param>
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware to handle the request and response.
    /// </summary>
    /// <param name="context">The HttpContext for the request.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Call the next middleware in the pipeline
            await _next(context);
        }
        catch
        {
            /* TODO:
             * log error
             */

            // Create an ErrorResponse object for internal server error
            var apiError = new ErrorResponse("Internal server error", []);

            // Set HTTP response status code to 500 (Internal Server Error)
            context.Response.StatusCode = 500;

            // Set content type to JSON
            context.Response.ContentType = "application/json";

            // Write the serialized ErrorResponse object to the response body
            await context.Response.WriteAsync(JsonConvert.SerializeObject(apiError));
        }
    }
}
