using CureFusion.Contracts.Articles;
using CureFusion.Contracts.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Abstactions;

namespace CureFusion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] ArticleQueryParameters queryParams, CancellationToken cancellationToken)
        {
            var result = await _articleService.GetAllArticlesAsync(queryParams, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("{id}", Name = "GetArticle")]
        public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _articleService.GetArticleByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromQuery] CreateArticleRequest request,[FromForm] string Content, [FromForm] UploadImageRequest articleImage, [FromQuery] string authorId, CancellationToken cancellationToken)
        {
            var result = await _articleService.CreateArticleAsync(request,Content, articleImage, authorId, cancellationToken);
            Console.WriteLine($"Generated ID: {result.Value!.Id}");
            return result.IsSuccess
                ? Created($"/Article/{result.Value!.Id}", result.Value)
                : result.ToProblem();
        }

    
        [HttpPut("")]
        public async Task<IActionResult> UpdateAsync([FromQuery] int id, [FromQuery] UpdateArticleRequest request, [FromForm] string Content,[FromForm] UploadImageRequest? articleImage, CancellationToken cancellationToken)
        {
            var result = await _articleService.UpdateArticleAsync(id, Content, request, articleImage, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

   
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _articleService.DeleteArticleAsync(id, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [HttpPut("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatusAsync([FromRoute] int id, [FromBody] UpdateArticleStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _articleService.UpdateArticleStatusAsync(id, request, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }
    }
}
