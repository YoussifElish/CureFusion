using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CureFusion.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HospitalsController : ControllerBase
{
        private readonly IGeoapifyService _geoapifyService;
        private readonly IGeoCoding _geoService;
        public HospitalsController(IGeoapifyService geoapifyService, IGeoCoding geoService)
        {
            _geoapifyService = geoapifyService;
            _geoService = geoService;
        }

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearbyHospitals(string zone, int radius = 5000)
        {
            try
            {
                // 🗺️ 1. Get the coordinates for the provided zone
                var (latitude, longitude) = await _geoService.GetCoordinatesAsync(zone);

                // 🏥 2. Find the nearby hospitals using Geoapify
                var hospitals = await _geoapifyService.GetNearbyHospitalsAsync(latitude, longitude, radius);

                // ✅ 3. Return the list of hospitals as a response
                return Ok(hospitals);
            }
            catch (Exception ex)
            {
                // ❌ If any error happens, we catch it and return a Bad Request with the error message
                return BadRequest(new { message = ex.Message });
            }
        }
    }

