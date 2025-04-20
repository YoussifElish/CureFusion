using CureFusion.Abstactions.Consts;
using CureFusion.Contracts.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Abstactions;

namespace CureFusion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DoctorController(IDoctorService doctorService) : ControllerBase
    {
        private readonly IDoctorService _doctorService = doctorService;
        [Authorize(Roles = DefaultRoles.Member)]

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsDoctor([FromBody] DoctorRegisterRequest request, CancellationToken cancellationToken = default)
        {
            
            var result = await _doctorService.RegisterAsDoctor(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem(); // TODO : Change it to Created at action after adding the get action
        }
        [Authorize(Roles = DefaultRoles.Doctor)]

        [HttpPost("AddDoctorAvaliability")]
        public async Task<IActionResult> AddDoctorAvaliability([FromBody] DoctorAvailabilityRequest request,  CancellationToken cancellationToken = default)
        {

            var result = await _doctorService.DoctorAvaliability(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem(); // TODO : Change it to Created at action after adding the get action
        }
        [Authorize(Roles = DefaultRoles.Doctor)]


        [HttpDelete("RemoveDoctorAvaliability/{id}")]

        public async Task<IActionResult> RemoveDoctorAvaliability([FromRoute] int id, CancellationToken cancellationToken = default)
        {

            var result = await _doctorService.DeleteDoctorAvaliability(id, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem(); 
        }
    }
}
