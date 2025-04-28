using CureFusion.Abstactions;
using CureFusion.Contracts.Articles;
using CureFusion.Contracts.Files;

namespace CureFusion.Services;

public interface IArticleService
{
    Task<Result<IEnumerable<ArticleResponse>>> GetAllArticlesAsync(ArticleQueryParameters queryParams, CancellationToken cancellationToken);
    Task<Result<ArticleResponse>> GetArticleByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<ArticleResponse>> CreateArticleAsync(CreateArticleRequest request, UploadImageRequest? articleImage, string authorId, CancellationToken cancellationToken);
    Task<Result> UpdateArticleAsync(int id, UpdateArticleRequest request, UploadImageRequest? articleImage, CancellationToken cancellationToken);
    Task<Result> UpdateArticleStatusAsync(int id, UpdateArticleStatusRequest request, CancellationToken cancellationToken);
    Task<Result> DeleteArticleAsync(int id, CancellationToken cancellationToken);
    Task<Result> IncrementArticleViewCountAsync(int id, CancellationToken cancellationToken); 
}

