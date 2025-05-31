using System.ComponentModel.DataAnnotations;
using CureFusion.Domain.Entities;

namespace CureFusion.Application.Contracts.Articles;

public record UpdateArticleStatusRequest(
    [Required]
    [EnumDataType(typeof(ArticleStatus))]
    string Status
);

