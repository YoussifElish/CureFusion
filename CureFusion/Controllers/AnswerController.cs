using CureFusion.Contracts.Answer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Abstactions;

namespace CureFusion.Controllers;
[Route("api/Questions/{QuestionId}/[controller]")]
[Authorize]
[ApiController]
public class AnswerController(IAnswerService answer) : ControllerBase
{
    private readonly IAnswerService _answer = answer;

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync([FromRoute] int Questionid, CancellationToken cancellationToken)
    {
        var result = await _answer.GetAllAnswers(Questionid, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateAsync([FromRoute] int QuestionId, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//fetishch the user id from the token

        var answer = await _answer.AddAnswer(QuestionId, request, userId!, cancellationToken);
        return answer.IsSuccess
            ? Ok(answer.Value)
            : answer.ToProblem();
    }
}
