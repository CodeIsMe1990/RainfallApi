using RainfallApi.Domain.Rainfall;

namespace RainfallApi.Application.Common.Interfaces;

/// <summary>
/// Interface for the Rainfall API service.
/// </summary>
public interface IRainfallApiService
{
    /// <summary>
    /// Lists rainfall readings by station ID asynchronously.
    /// </summary>
    /// <param name="stationId">The ID of the station.</param>
    /// <param name="limit">The maximum number of readings to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, returning a list of rainfall readings.</returns>
    Task<List<RainfallReading>> ListByStationIdAsync(string stationId, int limit, CancellationToken cancellationToken);
}
