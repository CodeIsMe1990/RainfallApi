using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using ErrorOr;
using MediatR;

using RainfallApi.Domain.Rainfall;

namespace RainfallApi.Application.Rainfall.Queries.ListRainfallReadings;

/// <summary>
/// Query record for listing rainfall readings.
/// </summary>
public record ListRainfallReadingsQuery(

    /// <summary>
    /// The ID of the station for which to list the rainfall readings.
    /// </summary>
    string StationId,

    /// <summary>
    /// The number of rainfall readings to retrieve. Must be between 1 and 100. Default is 10.
    /// </summary>
    [Range(1, 100)]
    [DefaultValue(10)]
    int Count) : IRequest<ErrorOr<List<RainfallReading>>>;
