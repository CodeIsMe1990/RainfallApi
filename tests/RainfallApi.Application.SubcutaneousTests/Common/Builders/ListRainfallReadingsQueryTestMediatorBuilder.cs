using FluentValidation.TestHelper;

using Moq;

using RainfallApi.Application.Rainfall.Queries.ListRainfallReadings;
using RainfallApi.Domain.Rainfall;

using TestCommon.Builders;

namespace RainfallApi.Application.SubcutaneousTests.Common.Builders;

/// <summary>
/// Builder class for creating a mediator instance for testing the list rainfall readings query handler.
/// </summary>
public class ListRainfallReadingsQueryTestMediatorBuilder
{
    protected CancellationTokenSource _cts;
    protected RainfallReadingsDataBuilder _dataBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListRainfallReadingsQueryTestMediatorBuilder"/> class with the specified cancellation token source.
    /// </summary>
    /// <param name="cts">The cancellation token source.</param>
    public ListRainfallReadingsQueryTestMediatorBuilder(CancellationTokenSource cts)
    {
        _cts = cts ?? new CancellationTokenSource();
        _dataBuilder = new RainfallReadingsDataBuilder();
    }

    /// <summary>
    /// Adds the provided data for the specified stationId to the builder.
    /// </summary>
    /// <param name="stationId">The station ID.</param>
    /// <param name="data">The rainfall readings data.</param>
    /// <returns>The current instance of the builder.</returns>
    public ListRainfallReadingsQueryTestMediatorBuilder WithData(string stationId, List<RainfallReading> data)
    {
        _dataBuilder.WithData(stationId, data);
        return this;
    }

    /// <summary>
    /// Adds random data for the specified stationId to the builder.
    /// </summary>
    /// <param name="stationId">The station ID.</param>
    /// <param name="count">The number of data entries to generate.</param>
    /// <returns>The current instance of the builder.</returns>
    public ListRainfallReadingsQueryTestMediatorBuilder WithRandomData(string stationId, int count)
    {
        _dataBuilder.WithRandomData(stationId, count);
        return this;
    }

    /// <summary>
    /// Builds and returns a mediator instance for testing the list rainfall readings query handler.
    /// </summary>
    /// <returns>A mock mediator instance.</returns>
    public IMediator GetTarget()
    {
        // Get the mock data for rainfall readings
        var dataMock = _dataBuilder.GetTarget();

        // Build the API service with the mock data
        var apiService = new RainfallApiServiceBuilder().WithData(dataMock).GetTarget();

        // Create the query handler with the API service
        var queryHandler = new ListRainfallReadingsQueryHandler(apiService);

        // Create the query validator
        var validator = new ListRainfallReadingsQueryValidator();

        // Create a mock for the mediator
        var mediatorStub = new Mock<IMediator>();

        // Set up the mediator to handle sending the query
        mediatorStub
            .Setup((m) => m.Send(It.IsAny<ListRainfallReadingsQuery>(), It.IsAny<CancellationToken>()))
            .Returns<ListRainfallReadingsQuery, CancellationToken>(async (request, cts) =>
            {
                // Validate the query
                var validationResult = await validator.TestValidateAsync(request, cancellationToken: cts);

                // Handle validation result in the mediator
                if (!validationResult.IsValid)
                {
                    // Create error instance with details based on validation errors
                    var errors = validationResult.Errors.Select(error =>
                        Error.Validation(error.PropertyName, error.ErrorMessage)).ToList();

                    return errors;
                }

                // If validation passes, handle the query with the query handler
                return await queryHandler.Handle(request, cts);
            });

        return mediatorStub.Object;
    }
}
