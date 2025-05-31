using CureFusion.Application.Services;

namespace CureFusion.API.Controllers;
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class QuestionController(IQuestionService question) : ControllerBase
{
    private readonly IQuestionService _question = question;



    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync([FromQuery] RequestFilter requestFilter, CancellationToken cancellationToken)
    {
        var result = await _question.GetAllQuestion(requestFilter, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("GetAsync/{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _question.GetQuestion(id, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("CreateAsync")]
    public async Task<IActionResult> CreateAsync([FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//fetishch the user id from the token


        var question = await _question.CreateQuestion(request, cancellationToken);
        return question.IsSuccess
            ? Ok(question.Value)
            : question.ToProblem();
    }

    [HttpPut("UpdateAsync{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _question.UpdateQuestion(id, request, cancellationToken);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    [HttpDelete("DeleteAsync/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _question.DeleteQuestionAsync(id, cancellationToken);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPost("UpVote/{QuestionId}")]
    public async Task<IActionResult> UpVote([FromRoute] int QuestionId, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//fetishch the user id from the token

        var answer = await _question.UpvoteQuestionAsync(QuestionId, cancellationToken);
        return answer.IsSuccess
            ? Ok()
            : answer.ToProblem();
    }


    [HttpPost("DownVote/{QuestionId}")]
    public async Task<IActionResult> DownVote([FromRoute] int QuestionId, [FromBody] AnswerRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//fetishch the user id from the token

        var answer = await _question.DownvoteQuestionAsync(QuestionId, cancellationToken);
        return answer.IsSuccess
            ? Ok()
            : answer.ToProblem();
    }
}


