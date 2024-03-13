using MediatR;

using Microsoft.AspNetCore.Http;

using Moq;

using RainfallApi.Api.Controllers;

namespace RainfallApi.Api.IntegrationTests.Common.ControllerBuilders;

/// <summary>
/// Builder class for constructing instances of <see cref="RainfallController"/>.
/// </summary>
public class RainfallControllerBuilder
{
    // Fields to hold mediator and HttpContext instances
    protected IMediator? _mediator;
    protected HttpContext? _httpContext;

    /// <summary>
    /// Sets the mediator instance for the builder.
    /// </summary>
    /// <param name="mediator">The mediator instance to set.</param>
    /// <returns>The current instance of <see cref="RainfallControllerBuilder"/>.</returns>
    public RainfallControllerBuilder WithMediator(IMediator mediator)
    {
        _mediator = mediator;
        return this;
    }

    /// <summary>
    /// Sets the HttpContext instance for the builder.
    /// </summary>
    /// <param name="httpContext">The HttpContext instance to set.</param>
    /// <returns>The current instance of <see cref="RainfallControllerBuilder"/>.</returns>
    public RainfallControllerBuilder WithHttpContext(HttpContext httpContext)
    {
        _httpContext = httpContext;
        return this;
    }

    /// <summary>
    /// Constructs an instance of <see cref="RainfallController"/> with configured dependencies.
    /// </summary>
    /// <returns>An instance of <see cref="RainfallController"/>.</returns>
    public RainfallController GetTarget()
    {
        // Creating a mock mediator if not provided
        var mediator = _mediator ?? new Mock<IMediator>().Object;

        // Creating a default HttpContext if not provided
        var httpContext = _httpContext ?? new DefaultHttpContext();

        // Creating a RainfallController instance with the configured mediator
        var result = new RainfallController(mediator);

        // Setting the HttpContext for the controller context
        result.ControllerContext.HttpContext = httpContext;

        return result;
    }
}