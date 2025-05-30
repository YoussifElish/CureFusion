﻿using CureFusion.Application.Contracts.Doctor;
using CureFusion.Application.Services;

namespace CureFusion.API.Controllers
{
    [Route("Doctor")]
    [ApiController]
    public class DoctorAppointmentController(IDoctorService doctorService, IAppointmentService appointmentService) : ControllerBase
    {
        private readonly IDoctorService _doctorService = doctorService;
        private readonly IAppointmentService _appointmentService = appointmentService;


        [Authorize(Roles = DefaultRoles.Member)]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsDoctor([FromForm] DoctorRegisterRequest request, [FromForm] RegisterDoctorImageRequest imageRequest, CancellationToken cancellationToken = default)
        {

            var result = await _doctorService.RegisterAsDoctor(request, imageRequest, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem(); // TODO : Change it to Created at action after adding the get action
        }

        [Authorize(Roles = DefaultRoles.Doctor)]
        [HttpPost("Appointment/AddDoctorAvaliability")]
        public async Task<IActionResult> AddDoctorAvaliability([FromBody] DoctorAvailabilityRequest request, CancellationToken cancellationToken = default)
        {

            var result = await _doctorService.DoctorAvaliability(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem(); // TODO : Change it to Created at action after adding the get action
        }

        [Authorize(Roles = DefaultRoles.Doctor)]
        [HttpDelete("Appointment/RemoveDoctorAvaliability/{id}")]
        public async Task<IActionResult> RemoveDoctorAvaliability([FromRoute] int id, CancellationToken cancellationToken = default)
        {

            var result = await _doctorService.DeleteDoctorAvaliability(id, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpDelete("Appointment/DeleteDoctorAppointment/{id}")]
        [Authorize(Roles = DefaultRoles.Doctor)]

        public async Task<IActionResult> DeleteDoctorAppointment([FromRoute] int id, CancellationToken cancellationToken = default)
        {

            var result = await _doctorService.DeleteDoctorAppointment(id, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [Authorize(Roles = DefaultRoles.Doctor)]
        [HttpDelete("Appointment/RemoveDoctorAppointment/{appointmentId}")]
        public async Task<IActionResult> CancelDoctorAppointment([FromRoute] int appointmentId, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.CancelAppointment(appointmentId, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpGet("GetAllDoctorAvailabilities")]
        [Authorize(Roles = DefaultRoles.Doctor)]

        public async Task<IActionResult> GetAllDoctorAvailabilities(CancellationToken cancellationToken)
        {
            var result = await _doctorService.GetDoctorAvailabilities(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("GetAllDoctorAppointments")]
        [Authorize(Roles = DefaultRoles.Doctor)]

        public async Task<IActionResult> GetDoctorAppointments(CancellationToken cancellationToken)
        {
            var result = await _doctorService.GetDoctorAppointments(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

    }
}
