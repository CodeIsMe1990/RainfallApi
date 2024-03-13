using System.Net.Http.Json;

using RainfallApi.Application.Common.Interfaces;
using RainfallApi.Application.Rainfall.Common;
using RainfallApi.Domain.Rainfall;

namespace RainfallApi.Infrastructure.Services;

/// <summary>
/// Service class responsible for retrieving rainfall readings from an external API.
/// </summary>
public class RainfallApiService : IRainfallApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RainfallApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Retrieves a list of rainfall readings for a specific station asynchronously.
    /// </summary>
    /// <param name="stationId">The ID of the station.</param>
    /// <param name="limit">The maximum number of readings to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of rainfall readings.</returns>
    public async Task<List<RainfallReading>> ListByStationIdAsync(string stationId, int limit, CancellationToken cancellationToken)
    {
        // Create a named client or use the default client
        HttpClient client = _httpClientFactory.CreateClient();

        // Construct the request URI with the specified station ID and limit
        string requestUri = string.Format("https://environment.data.gov.uk/flood-monitoring/id/stations/{0}/readings?_sorted&_limit={1}&parameter=rainfall", stationId, limit);

        try
        {
            // Make an HTTP GET request
            HttpResponseMessage response = await client.GetAsync(requestUri, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content into a RainfallApiServiceReadingResponse object
                var result = await response.Content.ReadFromJsonAsync<RainfallApiServiceReadingResponse>();

                // If the response is null, return an empty list of rainfall readings
                if (result == null)
                {
                    return new List<RainfallReading>();
                }

                // Map the response items to RainfallReading objects and return them as a list
                return result.Items.Select(rainfallReading => new RainfallReading(DateTime.Parse(rainfallReading.DateTime), new decimal(rainfallReading.Value))).ToList();
            }
            else
            {
                // Handle unsuccessful HTTP response
                throw new Exception($"Failed to retrieve rainfall readings. Status code: {response.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request exception
            throw new Exception("An error occurred while making the HTTP request.", ex);
        }
    }
}
