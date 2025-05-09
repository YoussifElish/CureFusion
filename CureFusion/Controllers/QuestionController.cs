
=======
﻿
>>>>>>> Stashed changes
namespace CureFusion.Controllers;
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class QuestionController(IQuestionService question) : ControllerBase
{
    private readonly IQuestionService _question = question;



    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync([FromQuery] RequestFilter requestFilter,CancellationToken cancellationToken)
    { 
    var result = await _question.GetAllQuestion(requestFilter, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            :result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _question.GetQuestion(id, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateAsync([FromBody ] QuestionRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//fetishch the user id from the token


        var question = await _question.CreateQuestion(request, userId, cancellationToken);
        return question.IsSuccess
            ? Ok(question.Value)
            : question.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _question.UpdateQuestion(id, request, cancellationToken);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _question.DeleteQuestionAsync(id, cancellationToken);
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
}


