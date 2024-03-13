namespace RainfallApi.Application.Rainfall.Common;

/// <summary>
/// Represents a rainfall reading in the Rainfall API.
/// </summary>
public record RainfallApiServiceReading(

    /// <summary>
    /// The date and time of the reading.
    /// </summary>
    string DateTime,

    /// <summary>
    /// The value of the rainfall reading.
    /// </summary>
    float Value);