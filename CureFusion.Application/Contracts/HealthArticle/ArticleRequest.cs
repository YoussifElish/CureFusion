namespace CureFusion.Application.Contracts.HealthArticle;

public record ArticleRequest
(

     string Category,
     string Title,
     string Content,
     DateTime PublishedIn
);

