using CureFusion.Contracts.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Abstactions;

namespace CureFusion.Controllers
{
    [Route("Patient/Appointment")]
    [ApiController]
    public class PatientAppointmentController(IAppointmentService appointmentService) : ControllerBase
    {
        private readonly IAppointmentService _appointmentService = appointmentService;

        //[Authorize]
        [HttpGet("GetActiveAppointments")]
        public async Task<IActionResult> GetActiveAppointments(CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetActiveAppointments(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }  
        
        [HttpGet("GetActiveAppointmentsByDoctorId")]
        public async Task<IActionResult> GetActiveAppointmentsByDoctorId([FromQuery]int id ,CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetActiveAppointmentsByDoctorId(id,cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        } 
        [HttpGet("GetDoctorsWithAppoitments")]
        public async Task<IActionResult> GetDoctorsWithAppoitments(CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetAllDoctorsWithAppoitments(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        //[Authorize]
        [HttpPost("Book")]
        public async Task<IActionResult> BookAppointment([FromBody] PatientAppointmentRequest request, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.BookAppointment(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }
    }
}