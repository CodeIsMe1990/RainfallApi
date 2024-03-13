using MediatR;

using Moq;

using RainfallApi.Application.Rainfall.Queries.ListRainfallReadings;

namespace TestCommon.Builders;

/// <summary>
/// Builder class for creating a mediator instance with custom request and result pairs.
/// </summary>
public class MediatorBuilder
{
    protected List<RequestAndResultPair> _requestAndResultPairs = new List<RequestAndResultPair>();

    /// <summary>
    /// Adds a request and result pair to the builder.
    /// </summary>
    /// <param name="requestAndResultPair">The request and result pair.</param>
    /// <returns>The current instance of the builder.</returns>
    public MediatorBuilder WithRequestAndResult(RequestAndResultPair requestAndResultPair)
    {
        _requestAndResultPairs.Add(requestAndResultPair);
        return this;
    }

    /// <summary>
    /// Builds and returns a mediator instance with the configured request and result pairs.
    /// </summary>
    /// <returns>A mock mediator instance.</returns>
    public IMediator GetTarget()
    {
        var resultStub = new Mock<IMediator>();

        // Set up each request and result pair
        _requestAndResultPairs.ForEach((x) =>
        {
            if (x.QueryType == typeof(ListRainfallReadingsQuery))
            {
                // Set up the mediator to return the specified value function when handling the query
                resultStub
                    .Setup(m => m.Send(It.IsAny<ListRainfallReadingsQuery>(), It.IsAny<CancellationToken>()))
                    .Returns(x.ValueFunction);
            }

            // Add additional conditions for handling different types of queries if needed
        });

        return resultStub.Object;
    }

    /// <summary>
    /// Represents a pair of request type and result delegate.
    /// </summary>
    public record RequestAndResultPair(Type QueryType, Delegate ValueFunction);
}