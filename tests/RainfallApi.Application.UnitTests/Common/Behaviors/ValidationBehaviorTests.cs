using FluentValidation;
using FluentValidation.Results;

using MediatR;

using RainfallApi.Application.Common.Behaviors;
using RainfallApi.Application.Rainfall.Queries.ListRainfallReadings;
using RainfallApi.Domain.Rainfall;

namespace RainfallApi.Application.UnitTests.Common.Behaviors;

/// <summary>
/// Unit tests for the validation behavior.
/// </summary>
public class ValidationBehaviorTests
{
    private readonly ValidationBehavior<ListRainfallReadingsQuery, ErrorOr<List<RainfallReading>>> _validationBehavior;
    private readonly IValidator<ListRainfallReadingsQuery> _mockValidator;
    private readonly RequestHandlerDelegate<ErrorOr<List<RainfallReading>>> _mockNextBehavior;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehaviorTests"/> class.
    /// </summary>
    public ValidationBehaviorTests()
    {
        // Creating a substitute for RequestHandlerDelegate
        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<List<RainfallReading>>>>();

        // Creating a substitute for IValidator
        _mockValidator = Substitute.For<IValidator<ListRainfallReadingsQuery>>();

        // Initializing ValidationBehavior with the mock validator
        _validationBehavior = new(_mockValidator);
    }

    /// <summary>
    /// Tests the behavior when the validator result is valid, the next behavior should be invoked.
    /// </summary>
    [Fact]
    public async Task InvokeValidationBehavior_WhenValidatorResultIsValid_ShouldInvokeNextBehavior()
    {
        // Arrange
        var stationId = "Station1";
        var count = 10;

        // Creating a ListRainfallReadingsQuery instance with specified stationId and count
        var listRainfallReadingsQuery = new ListRainfallReadingsQuery(stationId, count);

        // Creating a list of RainfallReading instances
        var rainfallReadings = new List<RainfallReading>()
    {
        new RainfallReading(DateTime.UtcNow, 0.1m),
        new RainfallReading(DateTime.UtcNow, 0.2m),
        new RainfallReading(DateTime.UtcNow, 0.3m),
    };

        // Setting up the mock validator to return a valid result
        _mockValidator
            .ValidateAsync(listRainfallReadingsQuery, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        // Setting up the mockNextBehavior to return the list of rainfallReadings
        _mockNextBehavior.Invoke().Returns(rainfallReadings);

        // Act
        var result = await _validationBehavior.Handle(listRainfallReadingsQuery, _mockNextBehavior, default);

        // Assert
        // Verifying that the result is not an error
        result.IsError.Should().BeFalse();

        // Verifying that the result value is equivalent to rainfallReadings
        result.Value.Should().BeEquivalentTo(rainfallReadings);
    }

    [Fact]
    public async Task InvokeValidationBehavior_WhenNoValidator_ShouldInvokeNextBehavior()
    {
        // Arrange
        var stationId = "Station1";
        var count = 10;

        // Creating a ListRainfallReadingsQuery instance with specified stationId and count
        var listRainfallReadingsQuery = new ListRainfallReadingsQuery(stationId, count);

        // Creating a new instance of ValidationBehavior without a validator
        var validationBehavior = new ValidationBehavior<ListRainfallReadingsQuery, ErrorOr<List<RainfallReading>>>();

        // Creating a list of RainfallReading instances
        var rainfallReadings = new List<RainfallReading>()
    {
        new RainfallReading(DateTime.UtcNow, 0.1m),
        new RainfallReading(DateTime.UtcNow, 0.2m),
        new RainfallReading(DateTime.UtcNow, 0.3m),
    };

        // Setting up the mockNextBehavior to return the list of rainfallReadings
        _mockNextBehavior.Invoke().Returns(rainfallReadings);

        // Act
        var result = await validationBehavior.Handle(listRainfallReadingsQuery, _mockNextBehavior, default);

        // Assert
        // Verifying that the result is not an error
        result.IsError.Should().BeFalse();

        // Verifying that the result value is equivalent to rainfallReadings
        result.Value.Should().BeEquivalentTo(rainfallReadings);
    }
}