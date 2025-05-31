// Controller to expose AdminService endpoints.

using CureFusion.Application.Contracts.Admin;
using CureFusion.Application.Services;

namespace CureFusion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")] // Secure endpoints for Admin role only
public class AdminController(IAdminService adminService) : ControllerBase
{
    private readonly IAdminService _adminService = adminService;

    // GET: api/Admin/dashboard-stats
    [HttpGet("dashboard-stats")]
    public async Task<IActionResult> GetDashboardStats(CancellationToken cancellationToken)
    {
        var result = await _adminService.GetDashboardStatsAsync(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    // GET: api/Admin/doctors
    [HttpGet("doctors")]
    public async Task<IActionResult> GetDoctors([FromQuery] AdminDoctorFilter filter, CancellationToken cancellationToken)
    {
        var result = await _adminService.GetDoctorsAsync(filter, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    // GET: api/Admin/doctors/{userId}
    [HttpGet("doctors/{userId}")]
    public async Task<IActionResult> GetDoctorDetails(string userId, CancellationToken cancellationToken)
    {
        var result = await _adminService.GetDoctorDetailsAsync(userId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error); // Use NotFound for specific item retrieval failure
    }

    // PUT: api/Admin/doctors/{userId}/status
    [HttpPut("doctors/{userId}/status")]
    public async Task<IActionResult> UpdateDoctorStatus(string userId, [FromBody] UpdateDoctorStatusRequest request, CancellationToken cancellationToken)
    {
        var result = await _adminService.UpdateDoctorStatusAsync(userId, request, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    // GET: api/Admin/patients
    [HttpGet("patients")]
    public async Task<IActionResult> GetPatients([FromQuery] AdminPatientFilter filter, CancellationToken cancellationToken)
    {
        var result = await _adminService.GetPatientsAsync(filter, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    // GET: api/Admin/appointments
    [HttpGet("appointments")]
    public async Task<IActionResult> GetAppointments([FromQuery] AdminAppointmentFilter filter, CancellationToken cancellationToken)
    {
        var result = await _adminService.GetAppointmentsAsync(filter, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    // Add endpoints for other admin functionalities (Reviews, Transactions, etc.) as needed
}

