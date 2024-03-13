using ErrorOr;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RainfallApi.Contracts.Common;

namespace RainfallApi.Api.Controllers;

/// <summary>
/// Base controller class for API controllers.
/// </summary>
[ApiController]
[Authorize]
public class ApiController : ControllerBase
{
    /// <summary>
    /// Handles error responses by returning appropriate HTTP responses based on the provided list of errors.
    /// </summary>
    /// <param name="errors">The list of errors to handle.</param>
    /// <returns>An ActionResult representing the appropriate HTTP response.</returns>
    protected ActionResult Problem(List<Error> errors)
    {
        // If no errors provided, return generic problem response
        if (errors.Count is 0)
        {
            return Problem();
        }

        // If all errors are of type Validation, return ValidationProblem response
        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        // For other error types, return Problem response with the first error
        return Problem(errors[0]);
    }

    /// <summary>
    /// Handles a specific error by returning an appropriate HTTP response with the provided error details.
    /// </summary>
    /// <param name="error">The error to handle.</param>
    /// <returns>An ObjectResult representing the appropriate HTTP response.</returns>
    private ObjectResult Problem(Error error)
    {
        // Create ErrorResponse object with error description
        var errorResponse = new ErrorResponse(error.Description, []);

        return error.Type switch
        {
            // For NotFound errors, return NotFound response with ErrorResponse
            ErrorType.NotFound => NotFound(errorResponse),

            // For other error types, return StatusCode response with ErrorResponse
            _ => StatusCode(500, errorResponse),
        };
    }

    /// <summary>
    /// Handles validation errors by returning a BadRequest response with the provided list of validation errors.
    /// </summary>
    /// <param name="errors">The list of validation errors to handle.</param>
    /// <returns>A BadRequest response with an ErrorResponse containing validation errors.</returns>
    private ActionResult ValidationProblem(List<Error> errors)
    {
        // Create list to store individual validation errors
        List<ErrorDetail> validationErrors = new List<ErrorDetail>();

        // Convert each error in the list to ErrorDetail and add to validationErrors list
        errors.ForEach(error => validationErrors.Add(new ErrorDetail(error.Code, error.Description)));

        // Return BadRequest response with ErrorResponse containing validation errors
        return BadRequest(new ErrorResponse("Invalid request.", validationErrors.ToArray()));
    }

    /*****************************************************
      \                                                 /
       >  Returning Microsoft default errors objects.  <
      /                                                 \
     *****************************************************
    *
    private ObjectResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };

        Problem(statusCode: statusCode, title: error.Description);
    }

    private ActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();
        return ValidationProblem(modelStateDictionary);
    }
*/
}