using CureFusion.Contracts.Doctor;
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

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsDoctor([FromBody] DoctorRegisterRequest request,string id, CancellationToken cancellationToken = default)
        {
            
            var result = await _doctorService.RegisterAsDoctor(request, id, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem(); // TODO : Change it to Created at action after adding the get action
        }


        [HttpPost("AddDoctorAvaliability")]
        public async Task<IActionResult> AddDoctorAvaliability([FromBody] DoctorAvailabilityRequest request, string id, CancellationToken cancellationToken = default)
        {

            var result = await _doctorService.DoctorAvaliability(request, id, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem(); // TODO : Change it to Created at action after adding the get action
        }


        [HttpDelete("RemoveDoctorAvaliability/{id}/{userId}")]

        public async Task<IActionResult> RemoveDoctorAvaliability([FromRoute] int id, [FromRoute]  string userId, CancellationToken cancellationToken = default)
        {

            var result = await _doctorService.DeleteDoctorAvaliability(id, userId, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem(); 
        }
    }
}
