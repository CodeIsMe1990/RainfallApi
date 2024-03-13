using FluentValidation;
using Microsoft.AspNetCore.Mvc;

using RainfallApi.Api.IntegrationTests.Common.Builders;
using RainfallApi.Application.Rainfall.Queries.ListRainfallReadings;
using RainfallApi.Contracts.Rainfall;

using TestCommon.Builders;

namespace RainfallApi.Api.IntegrationTests.Controllers;

/// <summary>
/// Test class for the ListReadings action in the RainfallController.
/// </summary>
public class ListReadingsTest
{
    protected CancellationTokenSource _cts;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListReadingsTest"/> class.
    /// </summary>
    public ListReadingsTest()
    {
        // Creating a cancellation token source
        _cts = new CancellationTokenSource();
    }

    /// <summary>
    /// Test method for verifying the behavior of the ListReadings action when the station ID is valid.
    /// </summary>
    /// <param name="stationId">The ID of the station.</param>
    /// <param name="count">The number of readings to retrieve.</param>
    [Theory]
    [InlineData("Station1", 10)]
    public async Task ListReadings_Given_StationIdIsValid_When_StationIdIsUsedToRequestRainfallReadings_Then_ReturnsOkObjectResult(string stationId, int count)
    {
        /*
         * Arrange
         */

        // Creating a mock dataset of rainfall readings
        var dataMock = new RainfallReadingsDataBuilder()
            .WithRandomData(stationId, 1)
            .GetTarget();

        // Creating a ListRainfallReadingsQuery object with stationId and count
        var listRainfallReadingsQuery = new ListRainfallReadingsQuery(stationId, count);

        // Building a RainfallControllerListReadingsTest object with the query and cancellation token source
        var test = await new RainfallControllerListReadingsTestBuilder(listRainfallReadingsQuery, _cts)
            .WithRainfallReadingsData(dataMock)
            .GetTarget();

        /*
         * Act
         */

        // Executing the action that needs testing and getting the result
        var actual = await test.ExecuteAct();

        /*
         * assert
         */

        // Verifying that the result is an OkObjectResult
        Assert.True(actual is OkObjectResult);
    }

    /// <summary>
    /// Test method to verify that when a valid station ID is used to request rainfall readings with a specific count,
    /// the API returns the correct amount of rainfall readings for that station.
    /// </summary>
    /// <param name="stationId">The ID of the station for which rainfall readings are requested.</param>
    /// <param name="count">The number of rainfall readings requested for the station.</param>
    [Theory]
    [InlineData("Station1", 1)]
    [InlineData("Station2", 100)]
    public async Task ListReadings_Given_StationIdIsValid_When_StationIdIsUsedToRequestRainfallReadings_Then_ReturnsCorrectAmountOfReadings(string stationId, int count)
    {
        /*
         * Arrange
         */

        // Creating a mock dataset of rainfall readings
        var dataMock = new RainfallReadingsDataBuilder()
            .WithRandomData("Station1", 2)
            .WithRandomData("Station2", 200)
            .GetTarget();

        // Calculating the expected number of rainfall readings based on the provided stationId and count
        var expectedRainfallReadingsCount = dataMock.Where(x => x.StationId == stationId).Single().RainfallReadings.Take(count).Count();

        // Creating a ListRainfallReadingsQuery object with stationId and count
        var listRainfallReadingsQuery = new ListRainfallReadingsQuery(stationId, count);

        // Building a RainfallControllerListReadingsTest object with the query and cancellation token source
        var test = await new RainfallControllerListReadingsTestBuilder(listRainfallReadingsQuery, _cts)
            .WithRainfallReadingsData(dataMock)
            .GetTarget();

        /*
         * Act
         */

        // Executing the action that needs testing and getting the result
        var actual = await test.ExecuteAct();

        /*
         * Assert
         */

        // Verifying that the result is an OkObjectResult
        Assert.True(actual is OkObjectResult);

        // Casting the result to OkObjectResult
        var okObjectResult = actual as OkObjectResult;
        Assert.NotNull(okObjectResult);

        // Casting the value of the OkObjectResult to RainfallReadingResponse
        var model = okObjectResult.Value as RainfallReadingResponse;
        Assert.NotNull(model);

        // Verifying that the 'Readings' property is not null or empty
        model.Readings.Should().NotBeNullOrEmpty();

        // Verifying that the number of readings returned matches the expected count
        model.Readings.Should().HaveCount(expectedRainfallReadingsCount);
    }

    /// <summary>
    /// Tests the behavior when the count is greater than the upper bound allowed for requesting rainfall readings.
    /// </summary>
    [Fact]
    public async Task ListReadings_Given_CountIsGreaterThanUpperBound_When_CountIsUsedToRequestRainfallReadings_Then_ReturnsBadRequestResponseWithCorrectErrorDetails()
    {
        /*
         * Arrange
         */

        var stationId = "Station1";
        var count = Constants.ListRainfallReadingQuery.CountUpperBound + 1;

        // Expected error message and count for validation failure
        var expectedErrorMessage = "Invalid request.";
        var expectedErrorDetailsCount = 1;
        var expectedErrorDetailMessage = "'Count' must be less than or equal to '100'.";

        // Creating a ListRainfallReadingsQuery object with stationId and count
        var listRainfallReadingsQuery = new ListRainfallReadingsQuery(stationId, count);

        // Building a RainfallControllerListReadingsTest object with the query and cancellation token source
        var listReadingsTest = await new RainfallControllerListReadingsTestBuilder(listRainfallReadingsQuery, _cts)
            .GetTarget();

        /*
         * Act
         */

        // Executing the action that needs testing and getting the result
        var actual = await listReadingsTest.ExecuteAct();

        /*
         * Assert
         */

        // Verifying that the result is an BadRequestObjectResult
        Assert.True(actual is BadRequestObjectResult);

        // Casting the result to BadRequestObjectResult
        var badRequestObjectResult = actual as BadRequestObjectResult;
        Assert.NotNull(badRequestObjectResult);

        // Casting the value of the BadRequestObjectResult to ErrorResponse
        var errorResponse = badRequestObjectResult.Value as ErrorResponse;
        Assert.NotNull(errorResponse);

        // Verifying the expected error message
        Assert.Equal(expectedErrorMessage, errorResponse.Message);

        // Verifying that error details are not null and have the expected count
        Assert.NotNull(errorResponse.Detail);
        Assert.Equal(expectedErrorDetailsCount, errorResponse.Detail.Length);

        // Verifying that the error detail message matches the expected error detail message
        Assert.Equal(errorResponse.Detail.First().Message, expectedErrorDetailMessage);
    }

    /// <summary>
    /// Tests the behavior when the count is less than the lower bound allowed for requesting rainfall readings.
    /// </summary>
    [Fact]
    public async Task ListReadings_Given_CountIsLessThanLowerBound_When_CountIsUsedToRequestRainfallReadings_Then_ReturnsBadRequestResponseWithCorrectErrorDetails()
    {
        /*
         * Arrange
         */

        var stationId = "Station1";
        var count = Constants.ListRainfallReadingQuery.CountLowerBound - 1;

        // Expected error message and count for validation failure
        var expectedErrorMessage = "Invalid request.";
        var expectedErrorDetailsCount = 1;
        var expectedErrorDetailMessage = "'Count' must be greater than or equal to '1'.";

        // Creating a ListRainfallReadingsQuery object with stationId and count
        var listRainfallReadingsQuery = new ListRainfallReadingsQuery(stationId, count);

        // Building a RainfallControllerListReadingsTest object with the query and cancellation token source
        var listReadingsTest = await new RainfallControllerListReadingsTestBuilder(listRainfallReadingsQuery, _cts)
            .GetTarget();

        /*
         * Act
         */

        // Executing the action that needs testing and getting the result
        var actual = await listReadingsTest.ExecuteAct();

        /*
         * Assert
         */

        // Verifying that the result is an BadRequestObjectResult
        Assert.True(actual is BadRequestObjectResult);

        // Casting the result to BadRequestObjectResult
        var badRequestObjectResult = actual as BadRequestObjectResult;
        Assert.NotNull(badRequestObjectResult);

        // Casting the value of the BadRequestObjectResult to ErrorResponse
        var errorResponse = badRequestObjectResult.Value as ErrorResponse;
        Assert.NotNull(errorResponse);

        // Verifying the expected error message
        Assert.Equal(expectedErrorMessage, errorResponse.Message);

        // Verifying that error details are not null and have the expected count
        Assert.NotNull(errorResponse.Detail);
        Assert.Equal(expectedErrorDetailsCount, errorResponse.Detail.Length);

        // Verifying that the error detail message matches the expected error detail message
        Assert.Equal(errorResponse.Detail.First().Message, expectedErrorDetailMessage);
    }

    /// <summary>
    /// Test method to verify that when an invalid station ID is used to request rainfall readings,
    /// the API returns a NotFound response with the correct error details.
    /// </summary>
    /// <param name="stationId">The invalid ID of the station for which rainfall readings are requested.</param>
    /// <param name="count">The count of rainfall readings requested.</param>
    [Theory]
    [InlineData("Station1", 10)]
    public async Task ListReadings_Given_StationIdIsInvalid_When_StationIdIsUsedToRequestRainfallReadings_Then_ReturnsNotFoundObjectResultWithCorrectDetails(string stationId, int count)
    {
        /*
         * Arrange
         */

        // Expected error message and count for validation failure
        var expectedErrorMessage = "No readings found for the specified stationId.";
        var expectedErrorDetailsCount = 0;

        // Creating a mock dataset of rainfall readings
        var dataMock = new RainfallReadingsDataBuilder()
            .WithRandomData("Station2", 1)
            .GetTarget();

        // Creating a ListRainfallReadingsQuery object with stationId and count
        var listRainfallReadingsQuery = new ListRainfallReadingsQuery(stationId, count);

        // Building a RainfallControllerListReadingsTest object with the query and cancellation token source
        var test = await new RainfallControllerListReadingsTestBuilder(listRainfallReadingsQuery, _cts)
            .WithRainfallReadingsData(dataMock)
            .GetTarget();

        /*
         * Act
         */

        // Executing the action that needs testing and getting the result
        var actual = await test.ExecuteAct();

        /*
         * assert
         */

        // Verifying that the result is a NotFoundObjectResult
        Assert.True(actual is NotFoundObjectResult);

        // Casting the result to NotFoundObjectResult
        var notFoundObjectResult = actual as NotFoundObjectResult;
        Assert.NotNull(notFoundObjectResult);

        // Casting the value of the NotFoundObjectResult to ErrorResponse
        var errorResponse = notFoundObjectResult.Value as ErrorResponse;
        Assert.NotNull(errorResponse);

        // Verifying the expected error message
        Assert.Equal(expectedErrorMessage, errorResponse.Message);

        // Verifying that error details are not null and have the expected count
        Assert.NotNull(errorResponse.Detail);
        Assert.Equal(expectedErrorDetailsCount, errorResponse.Detail.Length);
    }
}