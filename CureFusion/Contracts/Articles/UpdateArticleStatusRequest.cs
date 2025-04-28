namespace CureFusion.Contracts.Articles;

public record UpdateArticleStatusRequest(
    [Required]
    [EnumDataType(typeof(ArticleStatus))] 
    string Status
);

