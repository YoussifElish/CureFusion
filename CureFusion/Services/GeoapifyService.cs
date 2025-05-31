using System.Text.Json;
using CureFusion.Authentication.Filters;
using CureFusion.Contracts.Articles;
using Org.BouncyCastle.Crypto.Operators;

namespace CureFusion.Services;

public class GeoapifyService(HttpClient httpClient, IOptions<GeoapifyOptions> options) : IGeoapifyService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _apiKey = options.Value.ApiKey;

    public async Task<PageinatedList<Hospital>> GetNearbyHospitalsAsync(
     double latitude, double longitude, int radius = 5000, int pageNumber = 1, int pageSize = 10)
    {
        var url = $"https://api.geoapify.com/v2/places?categories=healthcare.hospital&filter=circle:{longitude},{latitude},{radius}&apiKey={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);

        var hospitals = new List<Hospital>();

        foreach (var feature in document.RootElement.GetProperty("features").EnumerateArray())
        {
            var properties = feature.GetProperty("properties");

            var location = new Location
            {
                Latitude = properties.GetProperty("lat").GetDouble(),
                Longitude = properties.GetProperty("lon").GetDouble()
            };
            var distance = DistanceCalculator.CalculateDistance(latitude, longitude, location.Latitude, location.Longitude);
            var zone = properties.TryGetProperty("suburb", out var suburbProp)
                     ? suburbProp.GetString() ?? ""
                     : properties.TryGetProperty("city", out var cityProp)
                     ? cityProp.GetString() ?? ""
                     : "";

            var hospital = new Hospital
            {
                Name = properties.GetProperty("name").GetString()!,
                Address = properties.GetProperty("address_line2").GetString()!,
                Zone = zone,
                Location = location,
                Distance = distance

            };

            hospitals.Add(hospital);
           
         
        }
        var sortedHospitals = hospitals.OrderBy(h => h.Distance).ToList();

       
        return PageinatedList<Hospital>.Create(sortedHospitals, pageNumber,pageSize);
    }
}