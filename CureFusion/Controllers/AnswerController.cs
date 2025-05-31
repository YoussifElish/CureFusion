using CureFusion.Application.Services;

namespace CureFusion.API.Controllers;
[Route("api/[controller]")]
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

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromRoute] int QuestionId, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {

        var answer = await _answer.AddAnswer(QuestionId, request, cancellationToken);
        return answer.IsSuccess
            ? Ok(answer.Value)
            : answer.ToProblem();
    }
    [Authorize(Roles = DefaultRoles.DoctorRoleId)]

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update([FromRoute] int QuestionId, [FromRoute] int id, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var answer = await _answer.UpdateAnswerAsync(id, QuestionId, request, cancellationToken);
        return answer.IsSuccess
            ? NoContent()
            : answer.ToProblem();
    }

    [Authorize(Roles = DefaultRoles.DoctorRoleId)]
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] int QuestionId, [FromRoute] int Id, CancellationToken cancellationToken)
    {
        var delete = await _answer.DeleteAnswerAsync(Id, QuestionId, cancellationToken);
        return delete.IsSuccess
            ? NoContent()
            : delete.ToProblem();
    }


    [HttpPost("UpVote/{AnswerId}")]
    public async Task<IActionResult> UpVote([FromRoute] int AnswerId, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {

        var answer = await _answer.UpvoteAnswerAsync(AnswerId, cancellationToken);
        return answer.IsSuccess
            ? Ok()
            : answer.ToProblem();
    }


    [HttpPost("DownVote/{AnswerId}")]
    public async Task<IActionResult> DownVote([FromRoute] int AnswerId, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {

        var answer = await _answer.DownvoteDAnswerAsync(AnswerId, cancellationToken);
        return answer.IsSuccess
            ? Ok()
            : answer.ToProblem();
    }
}
