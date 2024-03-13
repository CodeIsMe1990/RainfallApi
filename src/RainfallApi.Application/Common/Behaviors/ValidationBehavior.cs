using ErrorOr;

using FluentValidation;

using MediatR;

namespace RainfallApi.Application.Common.Behaviors;

/// <summary>
/// Pipeline behavior for request validation.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validator">The validator for the request.</param>
    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    /// <summary>
    /// Handles the request by performing validation and calling the next delegate in the pipeline.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // If no validator is provided, proceed to the next behavior
        if (_validator is null)
        {
            return await next();
        }

        // Validate the request using the provided validator
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        // If validation passes, proceed to the next behavior
        if (validationResult.IsValid)
        {
            return await next();
        }

        // Convert validation errors to Error objects
        var errors = validationResult.Errors
            .ConvertAll(error => Error.Validation(
                code: error.PropertyName,
                description: error.ErrorMessage));

        // Return the validation errors
        return (dynamic)errors;
    }
}

