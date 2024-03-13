namespace RainfallApi.Contracts.Rainfall;

/// <summary>
/// Represents a rainfall reading.
/// </summary>
public record RainfallReading(

    /// <summary>
    /// The date and time when the measurement was taken.
    /// </summary>
    string DateMeasured,

    /// <summary>
    /// The amount of rainfall measured.
    /// </summary>
    decimal AmountMeasured);