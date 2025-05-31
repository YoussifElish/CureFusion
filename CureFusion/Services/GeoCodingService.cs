
using System.Text.Json;

namespace CureFusion.Services;

public class GeocodingService(HttpClient httpClient) : IGeoCodingService
{
    private readonly string _apiKey = "dc11b12c242946abbe3aba432b4f9bdd";
    private readonly string _baseUrl = "https://api.opencagedata.com/geocode/v1/json";
    private readonly HttpClient _httpClient = httpClient;

    public async  Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string zone)
    {
        var encodedZone = Uri.EscapeDataString(zone);
        var url = $"{_baseUrl}?q={encodedZone}&key={_apiKey}";

        var response = await _httpClient.GetAsync(url); 
            response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);
           var results = document.RootElement.GetProperty("results");

        //after making the address an  array we loop through the results
        if (results.GetArrayLength() > 0)
        {
            var geometry = results[0].GetProperty("geometry");
            double latitude = geometry.GetProperty("lat").GetDouble(); 
                double longitude = geometry.GetProperty("lng").GetDouble();
            return (latitude, longitude);
        }

        throw new Exception("No coordinates found for the provided zone.");
    }
        }





