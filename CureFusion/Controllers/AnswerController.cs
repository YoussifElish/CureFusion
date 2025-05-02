using CureFusion.Abstactions;
using CureFusion.Abstactions.Consts;
using CureFusion.Contracts.Answer;
using CureFusion.Contracts.Common;
using CureFusion.Services;

namespace CureFusion.Controllers;
[Route("api/Questions/{QuestionId}/[controller]")]
[Authorize]
[ApiController]
public class AnswerController(IAnswerService answer) : ControllerBase
{
    private readonly IAnswerService _answer = answer;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int Questionid, CancellationToken cancellationToken)
    {
        var result = await _answer.GetAllAnswers(Questionid, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [Authorize(Roles = DefaultRoles.DoctorRoleId)]

    [HttpPost("")]
    public async Task<IActionResult> Create([FromRoute] int QuestionId, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//fetishch the user id from the token

        var answer = await _answer.AddAnswer(QuestionId, request, userId!, cancellationToken);
        return answer.IsSuccess
            ? Ok(answer.Value)
            : answer.ToProblem();
    }
    [Authorize(Roles = DefaultRoles.DoctorRoleId)]

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int QuestionId, [FromRoute] int id, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//fetishch the user id from the token
        var answer = await _answer.UpdateAnswerAsync(id, QuestionId, request, userId!, cancellationToken);
        return answer.IsSuccess
            ? NoContent()
            : answer.ToProblem();
    }

    [Authorize(Roles = DefaultRoles.DoctorRoleId)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int QuestionId, [FromRoute] int Id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//fetishch the user id from the token
        var delete = await _answer.DeleteAnswerAsync(Id, QuestionId, userId!, cancellationToken);
        return delete.IsSuccess
            ? NoContent()
            : delete.ToProblem();
    }
}
