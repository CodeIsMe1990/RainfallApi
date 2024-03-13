using ErrorOr;
using MediatR;
using RainfallApi.Application.Common.Interfaces;
using RainfallApi.Domain.Rainfall;

namespace RainfallApi.Application.Rainfall.Queries.ListRainfallReadings;

/// <summary>
/// Handler for the ListRainfallReadingsQuery.
/// </summary>
public class ListRainfallReadingsQueryHandler : IRequestHandler<ListRainfallReadingsQuery, ErrorOr<List<RainfallReading>>>
{
    private readonly IRainfallApiService _rainfallApiService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListRainfallReadingsQueryHandler"/> class.
    /// </summary>
    /// <param name="rainfallApiService">The service responsible for accessing rainfall data.</param>
    public ListRainfallReadingsQueryHandler(IRainfallApiService rainfallApiService)
    {
        _rainfallApiService = rainfallApiService;
    }

    /// <summary>
    /// Handles the ListRainfallReadingsQuery to retrieve rainfall readings.
    /// </summary>
    /// <param name="request">The query containing parameters for retrieving rainfall readings.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of rainfall readings or an error.</returns>
    public async Task<ErrorOr<List<RainfallReading>>> Handle(ListRainfallReadingsQuery request, CancellationToken cancellationToken)
    {
        // Call the IRainfallApiService to retrieve rainfall readings by station Id
        var rainfallReadings = await _rainfallApiService.ListByStationIdAsync(request.StationId, request.Count, cancellationToken);

        // Return the retrieved rainfall readings or an empty list if null
        return rainfallReadings ?? new List<RainfallReading>();
    }
}