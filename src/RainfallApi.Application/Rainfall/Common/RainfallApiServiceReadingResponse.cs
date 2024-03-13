namespace RainfallApi.Application.Rainfall.Common;

/// <summary>
/// Response record containing a list of <see cref="RainfallApiServiceReading"/> items.
/// </summary>
public record RainfallApiServiceReadingResponse(List<RainfallApiServiceReading> Items);
