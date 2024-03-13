using System.ComponentModel;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RainfallApi.Application.Rainfall.Queries.ListRainfallReadings;
using RainfallApi.Contracts.Common;
using RainfallApi.Contracts.Rainfall;

namespace RainfallApi.Api.Controllers
{
    /// <summary>
    /// Controller for managing rainfall data.
    /// </summary>
    [Route("[controller]")]
    [AllowAnonymous]
    public class RainfallController : ApiController
    {
        private readonly ISender _mediator;

        public RainfallController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get rainfall readings by station Id.
        /// </summary>
        /// <param name="stationId">The id of the reading station.</param>
        /// <param name="count">The number of readings to return.</param>
        /// <response code="200">A list of rainfall readings successfully retrieved.</response>
        /// <response code="400">Invalid request.</response>
        /// <response code="404">No readings found for the specified stationId.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("id/{stationId}/readings")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RainfallReadingResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> ListReadings([FromRoute] string stationId, [FromQuery][DefaultValue(10)] int count = 10)
        {
            // List to store validation errors for unexpected query keys
            var unexpectedQueryKeyErrors = new List<Error>();

            // List of query keys that are expected
            var availableQueryKeys = new List<string> { nameof(count) };

            // Get unexpected query keys
            var unexpectedQueryKeys = HttpContext.Request.Query.Select(x => x.Key).Except(availableQueryKeys).ToList();

            // Generate validation errors for unexpected query keys
            unexpectedQueryKeys.ForEach(key => unexpectedQueryKeyErrors.Add(Error.Validation(code: key, description: string.Format("Unexpected parameter: '{0}'", key))));

            // Return 400 Bad Request if there are validation errors
            if (unexpectedQueryKeyErrors.Count > 0)
            {
                return Problem(unexpectedQueryKeyErrors);
            }

            // Create query object
            var query = new ListRainfallReadingsQuery(stationId, count);

            // Send query using Mediator pattern
            var result = await _mediator.Send(query);

            // Handle case where no readings are found for the specified stationId
            if (result.Value?.Count == 0)
            {
                return Problem(new List<Error> { Error.NotFound(description: "No readings found for the specified stationId.") });
            }

            // Return Ok with rainfall readings
            return result.Match(
                readings => Ok(new RainfallReadingResponse([.. readings.ConvertAll(ToDto)])),
                Problem);
        }

        // Convert domain object to DTO
        private RainfallReading ToDto(Domain.Rainfall.RainfallReading reading) =>
            new(reading.DateMeasuredUtc.ToString("s") + "Z", reading.AmountMeasured);
    }
}