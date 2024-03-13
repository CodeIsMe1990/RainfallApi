namespace RainfallApi.Contracts.Rainfall;

/// <summary>
/// Details of a rainfall reading.
/// </summary>
public record RainfallReadingResponse
(
    RainfallReading[] Readings);