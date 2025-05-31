using CureFusion.Application.Services;

namespace CureFusion.API.Controllers
{
    [Route("admin/appointment")]
    [ApiController]
    public class AdminAppointmentController(IAppointmentService appointmentService) : ControllerBase
    {
        private readonly IAppointmentService _appointmentService = appointmentService;

        [Authorize(Roles = (DefaultRoles.Admin))]
        [HttpGet("GetAllAppointments")]
        public async Task<IActionResult> GetAllAppointments(CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetAllAppointments(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
