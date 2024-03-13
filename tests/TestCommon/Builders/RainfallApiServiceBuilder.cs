using Moq;

using RainfallApi.Application.Common.Interfaces;
using RainfallApi.Domain.Rainfall;

using TestCommon.Rainfall;

namespace TestCommon.Builders;

/// <summary>
/// Builder class for creating mock instances of IRainfallApiService for testing purposes.
/// </summary>
public class RainfallApiServiceBuilder
{
    // List to store data for mock service
    protected List<StationRainfallReadings> _data = new List<StationRainfallReadings>();

    /// <summary>
    /// Set the data for the mock service.
    /// </summary>
    /// <param name="data">The data to be used for mocking the service.</param>
    /// <returns>The current instance of the builder.</returns>
    public RainfallApiServiceBuilder WithData(List<StationRainfallReadings> data)
    {
        _data = data;
        return this;
    }

    /// <summary>
    /// Build and return a mock instance of IRainfallApiService.
    /// </summary>
    /// <returns>A mock instance of IRainfallApiService.</returns>
    public IRainfallApiService GetTarget()
    {
        // Create a mock instance of IRainfallApiService
        var resultStub = new Mock<IRainfallApiService>();

        // Setup behavior for ListByStationIdAsync method
        resultStub
            .Setup((m) => m.ListByStationIdAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns<string, int, CancellationToken>((stationId, limit, cts) =>
            {
                // Filter the _data list to get StationRainfallReadings objects with the specified stationId
                var stationData = _data.Where(data => data.StationId == stationId).ToList()
                    .SelectMany(data => data.RainfallReadings)
                    .OrderByDescending(reading => reading.DateMeasuredUtc)
                    .Take(limit)
                    .ToList();
                return Task.FromResult(stationData);
            });

        // Return the mock instance of IRainfallApiService
        return resultStub.Object;
    }
}