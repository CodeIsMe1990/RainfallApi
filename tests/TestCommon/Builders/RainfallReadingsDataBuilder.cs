using RainfallApi.Domain.Rainfall;

using TestCommon.Rainfall;

namespace TestCommon.Builders;

/// <summary>
/// Builder class for creating rainfall readings data for testing.
/// </summary>
public class RainfallReadingsDataBuilder
{
    protected List<StationRainfallReadings> _stationRainfallReadings = new List<StationRainfallReadings>();

    /// <summary>
    /// Adds the provided rainfall readings data to the builder.
    /// </summary>
    /// <param name="rainfallReadings">The rainfall readings data.</param>
    /// <returns>The current instance of the builder.</returns>
    public RainfallReadingsDataBuilder WithData(List<StationRainfallReadings> rainfallReadings)
    {
        _stationRainfallReadings.AddRange(rainfallReadings);
        return this;
    }

    /// <summary>
    /// Adds rainfall readings data for the specified stationId to the builder.
    /// </summary>
    /// <param name="stationId">The station ID.</param>
    /// <param name="rainfallReadings">The rainfall readings data.</param>
    /// <returns>The current instance of the builder.</returns>
    public RainfallReadingsDataBuilder WithData(string stationId, List<RainfallReading> rainfallReadings)
    {
        _stationRainfallReadings.Add(new StationRainfallReadings(stationId, rainfallReadings));
        return this;
    }

    /// <summary>
    /// Adds randomly generated rainfall readings data for the specified station ID and count to the builder.
    /// </summary>
    /// <param name="stationId">The station ID.</param>
    /// <param name="count">The number of data entries to generate.</param>
    /// <returns>The current instance of the builder.</returns>
    public RainfallReadingsDataBuilder WithRandomData(string stationId, int count)
    {
        var minValue = 0;
        var maxValue = 4;

        // Create a new instance of the Random class to generate random values
        Random random = new Random();

        // Create a list to store the generated rainfall readings
        var readings = new List<RainfallReading>();

        // Loop to generate the specified count of rainfall readings
        for (int i = 0; i < count; i++)
        {
            // Generate a random value between 0 and 1
            var next = random.NextDouble();

            // Scale the random value to the desired range (minValue to maxValue)
            var measuredAmount = minValue + (next * (maxValue - minValue));

            // Create a new RainfallReading object with the current date and the generated measured amount
            readings.Add(new RainfallReading(DateTime.UtcNow, new decimal(measuredAmount)));
        }

        // Add the generated rainfall readings for the specified station ID to the list
        _stationRainfallReadings.Add(new StationRainfallReadings(stationId, readings));

        // Return the current instance of the builder
        return this;
    }

    /// <summary>
    /// Builds and returns the rainfall readings data by merging and sorting the readings for each station.
    /// </summary>
    /// <returns>The list of merged and sorted station rainfall readings.</returns>
    public List<StationRainfallReadings> GetTarget()
    {
        // Group the station rainfall readings by station ID, merge the readings, and sort them in descending order of date measured UTC
        var mergedStationReadings = _stationRainfallReadings
            .GroupBy(sr => sr.StationId)
            .Select(g => new StationRainfallReadings(g.Key, g.SelectMany(sr => sr.RainfallReadings).OrderByDescending(o => o.DateMeasuredUtc).ToList()))
            .OrderBy(o => o.StationId)
            .ToList();

        // Return the list of merged and sorted station rainfall readings
        return new List<StationRainfallReadings>(mergedStationReadings);
    }
}