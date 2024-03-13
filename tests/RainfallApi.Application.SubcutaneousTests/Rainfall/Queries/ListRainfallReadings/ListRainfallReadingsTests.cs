using RainfallApi.Application.Rainfall.Queries.ListRainfallReadings;
using RainfallApi.Application.SubcutaneousTests.Common.Builders;
using RainfallApi.Domain.Rainfall;

using TestCommon.TestConstants;

namespace RainfallApi.Application.SubcutaneousTests.Rainfall.Queries.ListRainfallReadings;

/// <summary>
/// Tests the behavior when query is provided, and the method should return rainfall readings.
/// </summary>
public class ListRainfallReadingsTests
{
    protected CancellationTokenSource _cts;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListRainfallReadingsTests"/> class.
    /// </summary>
    public ListRainfallReadingsTests()
    {
        _cts = new CancellationTokenSource();
    }

    /// <summary>
    /// Tests the behavior of the list rainfall readings query handler when provided with a valid query, ensuring that it returns the expected rainfall readings.
    /// </summary>
    [Fact]
    public async Task ListRainfallReadings_WhenValidQuery_ShouldReturnRainfallReadings()
    {
        // Arrange
        var stationId = "Station1";
        var count = 10;

        // Get the current UTC time
        var dateTimeUtc = DateTime.UtcNow;

        // Define the expected data
        var expectedData = new List<RainfallReading>
        {
            new RainfallReading(dateTimeUtc + TimeSpan.FromMinutes(300), 0.1m),
            new RainfallReading(dateTimeUtc + TimeSpan.FromMinutes(270), 0.2m),
            new RainfallReading(dateTimeUtc + TimeSpan.FromMinutes(240), 0.3m),
            new RainfallReading(dateTimeUtc + TimeSpan.FromMinutes(210), 0.4m),
            new RainfallReading(dateTimeUtc + TimeSpan.FromMinutes(180), 0.5m),
            new RainfallReading(dateTimeUtc + TimeSpan.FromMinutes(150), 0.6m),
            new RainfallReading(dateTimeUtc + TimeSpan.FromMinutes(120), 0.7m),
            new RainfallReading(dateTimeUtc + TimeSpan.FromMinutes(90), 0.8m),
            new RainfallReading(dateTimeUtc + TimeSpan.FromMinutes(60), 0.9m),
            new RainfallReading(dateTimeUtc + TimeSpan.FromMinutes(30), 1.0m),
        };

        // Build the mediator for testing
        var mediator = new ListRainfallReadingsQueryTestMediatorBuilder(_cts)
            .WithRandomData(stationId, 40)
            .WithRandomData("Station2", 5)
            .WithData(stationId, expectedData)
            .GetTarget();

        // Create the query
        var query = new ListRainfallReadingsQuery(stationId, count);

        // Act
        var actual = await mediator.Send(query, _cts.Token);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeEquivalentTo(expectedData);
    }

    /// <summary>
    /// Tests the behavior of the list rainfall readings query handler when there are no rainfall readings, ensuring that it returns an empty list.
    /// </summary>
    [Fact]
    public async Task ListRainfallReadings_WhenNoRainfallReadings_ShouldReturnEmptyList()
    {
        // Arrange
        var stationId = "Station1";
        var count = 10;

        // Build the mediator for testing
        var mediator = new ListRainfallReadingsQueryTestMediatorBuilder(_cts)
            .GetTarget();

        // Create the query
        var query = new ListRainfallReadingsQuery(stationId, count);

        // Act
        var actual = await mediator.Send(query);

        // Assert
        actual.IsError.Should().BeFalse();
        actual.Value.Should().BeEmpty();
    }

    /// <summary>
    /// Tests the behavior of the list rainfall readings query handler when the count provided exceeds the upper bound, ensuring that it returns errors with correct details.
    /// </summary>
    [Fact]
    public async Task ListRainfallReadings_WhenCountIsGreaterThanUpperBound_ShouldReturnErrorsWithCorrectDetails()
    {
        var stationId = "Station1";
        var count = Constants.ListRainfallReadingQuery.CountUpperBound + 1;

        // Expected error code and error description for validation failure
        var expectedErrorCode = "count";
        var expectedErrorDescription = "'Count' must be less than or equal to '100'.";

        // Build the mediator for testing
        var mediator = new ListRainfallReadingsQueryTestMediatorBuilder(_cts)
            .WithRandomData(stationId, 10)
            .GetTarget();

        // Create the query
        var query = new ListRainfallReadingsQuery(stationId, count);

        // Act
        var actual = await mediator.Send(query);

        // Assert
        actual.IsError.Should().BeTrue();
        actual.Errors.Should().NotBeEmpty();
        actual.Errors.First().Code.Should().BeEquivalentTo(expectedErrorCode);
        actual.Errors.First().Description.Should().BeEquivalentTo(expectedErrorDescription);
    }

    /// <summary>
    /// Tests the behavior of the list rainfall readings query handler when the count provided is less than the lower bound, ensuring that it returns errors with correct details.
    /// </summary>
    [Fact]
    public async Task ListRainfallReadings_WhenCountIsLessThanLowerBound_ShouldReturnErrorsWithCorrectDetails()
    {
        var stationId = "Station1";
        var count = Constants.ListRainfallReadingQuery.CountLowerBound - 1;

        // Expected error code and error description for validation failure
        var expectedErrorCode = "count";
        var expectedErrorDescription = "'Count' must be greater than or equal to '1'.";

        // Build the mediator for testing
        var mediator = new ListRainfallReadingsQueryTestMediatorBuilder(_cts)
            .WithRandomData(stationId, 10)
            .GetTarget();

        // Create the query
        var query = new ListRainfallReadingsQuery(stationId, count);

        // Act
        var actual = await mediator.Send(query);

        // Assert
        actual.IsError.Should().BeTrue();
        actual.Errors.Should().NotBeEmpty();
        actual.Errors.First().Code.Should().BeEquivalentTo(expectedErrorCode);
        actual.Errors.First().Description.Should().BeEquivalentTo(expectedErrorDescription);
    }
}