using CureFusion.Abstactions.Consts;
using CureFusion.Contracts.Files;
using CureFusion.Contracts.Medicine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Abstactions;

namespace CureFusion.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DrugController(IDrugService drug) : ControllerBase
{
  

        private readonly IDrugService _drug = drug;

        [HttpGet("GetALl")]
        public async Task<IActionResult> GetAllasync(CancellationToken cancellationToken)
        {
            var Result = await _drug.GetAllDrugAsync(cancellationToken);
            return Result.IsSuccess ? Ok(Result.Value) : Result.ToProblem();
          
        }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Getasync([FromRoute]int id,CancellationToken cancellationToken)
    {
        var Result = await _drug.GetDrugAsync(id,cancellationToken);
        return Result.IsSuccess
            ? Ok(Result.Value)
            : Result.ToProblem();
    
    }
    [Authorize(Roles =DefaultRoles.AdminRoleId+","+ DefaultRoles.DoctorRoleId)]
    [HttpPost("Add")]
    public async Task<IActionResult> AddAsync([FromForm] DrugRequest Request, [FromForm] UploadImageRequest drugImage, CancellationToken cancellationToken)
    {
        var Result = await _drug.AddDrugAsync(Request, drugImage, cancellationToken);
        return Result.IsSuccess 
            ? CreatedAtAction(nameof(Getasync), new { id = Result.Value!.Id },Result.Value)
            : Result.ToProblem();

    }
    [Authorize(Roles =DefaultRoles.AdminRoleId+","+ DefaultRoles.DoctorRoleId)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id,[FromBody] DrugRequest Request, CancellationToken cancellationToken)
    {
        var Result = await _drug.UpdateDrugAsync(id,Request, cancellationToken);
        return Result.IsSuccess 
            ? NoContent()
            : Result.ToProblem();
        //i will check it again 

    }
    [Authorize(Roles =DefaultRoles.AdminRoleId+","+ DefaultRoles.DoctorRoleId)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var Result = await _drug.DeleteDrugAsync(id, cancellationToken);
        return Result.IsSuccess
            ? NoContent()
            : Result.ToProblem();
     

    }
}


