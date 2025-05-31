using CureFusion.Application.Contracts.Articles;
using CureFusion.Application.Contracts.Files;
using CureFusion.Domain.Abstactions;

namespace CureFusion.Application.Services;

public interface IArticleService
{
    Task<Result<IEnumerable<ArticleResponse>>> GetAllArticlesAsync(ArticleQueryParameters queryParams, CancellationToken cancellationToken);
    Task<Result<ArticleResponse>> GetArticleByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<ArticleResponse>> CreateArticleAsync(CreateArticleRequest request, string Content, UploadImageRequest? articleImage, string authorId, CancellationToken cancellationToken);
    Task<Result> UpdateArticleAsync(int id, string content, UpdateArticleRequest request, UploadImageRequest? articleImage, CancellationToken cancellationToken);
    Task<Result> UpdateArticleStatusAsync(int id, UpdateArticleStatusRequest request, CancellationToken cancellationToken);
    Task<Result> DeleteArticleAsync(int id, CancellationToken cancellationToken);
    Task<Result> IncrementArticleViewCountAsync(int id, CancellationToken cancellationToken);
}

