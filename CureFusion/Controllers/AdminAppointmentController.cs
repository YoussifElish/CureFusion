using CureFusion.Abstactions.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Abstactions;

namespace CureFusion.Controllers
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
