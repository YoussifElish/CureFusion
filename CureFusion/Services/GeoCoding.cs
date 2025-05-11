using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CureFusion.Contracts.Hospital;


namespace CureFusion.Services
{
    public class GeocodingService(HttpClient httpClient) : IGeoCoding
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly string _apiKey = "dc11b12c242946abbe3aba432b4f9bdd";
        private readonly string _baseUrl = "https://api.opencagedata.com/geocode/v1/json";

        public async Task<Location> GetCoordinatesAsync(string address)
        {
           
            var url = $"{_baseUrl}?q={address}&key={_apiKey}";

            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            //  to Check if the response is successful

            // Read the response content as a string
            var json = await response.Content.ReadAsStringAsync();

            // Parse the JSON response
            using var document = JsonDocument.Parse(json);
            var results = document.RootElement.GetProperty("results");

            //after making the address an  array we loop through the results
            if (results.GetArrayLength() > 0)
            {
                var geometry = results[0].GetProperty("geometry");
                double latitude = geometry.GetProperty("lat").GetDouble();  
                double longitude = geometry.GetProperty("lng").GetDouble();  

                // Return a new Location object with the extracted coordinates
              return new Location
              {
                  Latitude = latitude,
                  Longitude = longitude
              };
            };
        
            return null;
        }


    }
    }


