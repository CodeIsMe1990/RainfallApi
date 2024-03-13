using RainfallApi.Domain.Rainfall;

namespace TestCommon.Rainfall;

/// <summary>
/// Represents rainfall readings for a station.
/// </summary>
public record StationRainfallReadings(string StationId, List<RainfallReading> RainfallReadings);
