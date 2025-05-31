using CureFusion.Domain.Entities;

namespace CureFusion.Application.Contracts.Articles;

public record ArticleResponse(
    int Id,
    string Title,
    string Summary,
    string Content,
    ArticleCategory Category,
    string Tags,
    string Status,
    DateTime PublishedDate,
    int ViewCount,
    string AuthorName,
    string authorId,
    string? ImageUrl
);
