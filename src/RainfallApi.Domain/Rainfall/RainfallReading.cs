using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainfallApi.Domain.Rainfall;

/// <summary>
/// Represents a rainfall reading.
/// </summary>
public class RainfallReading
{
    /// <summary>
    /// Gets or sets the date and time when the rainfall was measured in UTC.
    /// </summary>
    public DateTime DateMeasuredUtc { get; set; }

    /// <summary>
    /// Gets or sets the amount of rainfall measured.
    /// </summary>
    public decimal AmountMeasured { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RainfallReading"/> class with the specified date and amount measured.
    /// </summary>
    /// <param name="dateMeasuredUtc">The date and time when the rainfall was measured in UTC.</param>
    /// <param name="amountMeasured">The amount of rainfall measured.</param>
    public RainfallReading(DateTime dateMeasuredUtc, decimal amountMeasured)
    {
        DateMeasuredUtc = dateMeasuredUtc;
        AmountMeasured = amountMeasured;
    }

    // Private parameterless constructor to prevent external instantiation without parameters
    private RainfallReading()
    {
    }
}