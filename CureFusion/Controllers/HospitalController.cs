using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CureFusion.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HospitalController(IGeoapifyService geoapifyService, IGeoCodingService geoService) : ControllerBase
{
    private readonly IGeoapifyService _geoapifyService = geoapifyService;
    private readonly IGeoCodingService _geoService = geoService;

    [HttpGet("nearby")]
    public async Task<IActionResult> GetNearbyHospitals([FromQuery] string zone, [FromQuery] int radius = 5000, [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        try
        {

            var (latitude, longitude) = await _geoService.GetCoordinatesAsync(zone);

            var hospitals = await _geoapifyService.GetNearbyHospitalsAsync(latitude, longitude, radius);

            if (hospitals == null)
                return BadRequest();

            return Ok(hospitals);
        }
        catch (Exception ex)
        {
            // Log the exception (ex) if you have a logging mechanism
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");


        }
    }
}
