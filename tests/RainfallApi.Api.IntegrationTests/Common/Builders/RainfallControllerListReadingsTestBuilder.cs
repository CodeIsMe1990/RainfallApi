using ErrorOr;

using FluentValidation.TestHelper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RainfallApi.Api.Controllers;
using RainfallApi.Api.IntegrationTests.Common.ControllerBuilders;
using RainfallApi.Application.Rainfall.Queries.ListRainfallReadings;

using TestCommon.Builders;
using TestCommon.Rainfall;

namespace RainfallApi.Api.IntegrationTests.Common.Builders;

/// <summary>
/// Builder class for setting up integration tests for RainfallController's ListReadings action.
/// </summary>
public class RainfallControllerListReadingsTestBuilder
{
    // Fields to store builder components
    protected CancellationTokenSource _cts;
    protected RainfallReadingsDataBuilder _rainfallReadingsDataBuilder;
    protected ListRainfallReadingsQuery _listRainfallReadingsQuery;
    protected RainfallApiServiceBuilder _rainfallApiServiceBuilder;
    protected MediatorBuilder _mediatorBuilder;
    protected RainfallControllerBuilder _rainfallControllerBuilder;

    public RainfallControllerListReadingsTestBuilder(ListRainfallReadingsQuery listRainfallReadingsQuery, CancellationTokenSource? cts)
    {
        // Initialize builder components
        _rainfallReadingsDataBuilder = new RainfallReadingsDataBuilder();
        _rainfallApiServiceBuilder = new RainfallApiServiceBuilder();
        _mediatorBuilder = new MediatorBuilder();
        _rainfallControllerBuilder = new RainfallControllerBuilder();

        // Assign cancellation token source
        _cts = cts ?? new CancellationTokenSource();
        _listRainfallReadingsQuery = listRainfallReadingsQuery;
    }

    /// <summary>
    /// Set rainfall readings data for the test.
    /// </summary>
    /// <param name="data">data.</param>
    public RainfallControllerListReadingsTestBuilder WithRainfallReadingsData(List<StationRainfallReadings> data)
    {
        _rainfallReadingsDataBuilder.WithData(data);
        return this;
    }

    /// <summary>
    /// Build and return the integration test instance.
    /// </summary>
    public async Task<RainfallControllerListReadingsTest> GetTarget()
    {
        // Retrieve rainfall data
        var rainfallData = _rainfallReadingsDataBuilder.GetTarget();

        // Create mock rainfall API service
        var rainfallApiService = _rainfallApiServiceBuilder
            .WithData(rainfallData)
            .GetTarget();

        // Create listRainfallReadingsQuery instance
        var listRainfallReadingsQuery = new ListRainfallReadingsQuery(_listRainfallReadingsQuery.StationId, _listRainfallReadingsQuery.Count);

        // Validator for validating the request
        var validator = new ListRainfallReadingsQueryValidator();
        var validationResult = await validator.TestValidateAsync(listRainfallReadingsQuery);

        // Request handler for handling the query
        var requestHandler = new ListRainfallReadingsQueryHandler(rainfallApiService);

        // Mediator setup for handling requests and results
        var mediator = new MediatorBuilder()
            .WithRequestAndResult(
                new MediatorBuilder.RequestAndResultPair(typeof(ListRainfallReadingsQuery), async () =>
                {
                    // Handle validation result in the mediator
                    if (!validationResult.IsValid)
                    {
                        // Create error instance with details based on validation errors
                        var errors = validationResult.Errors.Select(error =>
                            Error.Validation(error.PropertyName, error.ErrorMessage)).ToList();

                        return errors;
                    }

                    return await requestHandler.Handle(listRainfallReadingsQuery, _cts.Token);
                }))
            .GetTarget();

        // Controller setup with mocked HttpContext and mediator
        var rainfallController = _rainfallControllerBuilder
            .WithHttpContext(new DefaultHttpContext() { RequestAborted = _cts.Token })
            .WithMediator(mediator)
            .GetTarget();

        // Return the integration test instance
        return new RainfallControllerListReadingsTest(rainfallData, listRainfallReadingsQuery, rainfallController);
    }
}

/// <summary>
/// Integration test class for RainfallController's ListReadings action.
/// </summary>
public class RainfallControllerListReadingsTest
{
    // Fields to store test data and controller instance
    protected List<StationRainfallReadings> _dataMock;
    protected ListRainfallReadingsQuery _listReadingsQuery;
    protected RainfallController _rainfallController;

    // Properties to access test data and controller instance
    public List<StationRainfallReadings> DataMock => _dataMock;
    public RainfallController Controller => _rainfallController;

    public RainfallControllerListReadingsTest(
        List<StationRainfallReadings> dataMock,
        ListRainfallReadingsQuery listRainfallReadingsQuery,
        RainfallController rainfallController)
    {
        _dataMock = dataMock;
        _listReadingsQuery = listRainfallReadingsQuery;
        _rainfallController = rainfallController;
    }

    /// <summary>
    /// Execute the action and return the IActionResult result.
    /// </summary>
    public async Task<IActionResult> ExecuteAct()
    {
        return await _rainfallController.ListReadings(_listReadingsQuery.StationId, _listReadingsQuery.Count);
    }
}