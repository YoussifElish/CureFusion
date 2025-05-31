using System.ComponentModel.DataAnnotations;
using CureFusion.Domain.Entities;

namespace CureFusion.Application.Contracts.Articles;

public record CreateArticleRequest(
    [Required]
    [StringLength(200, MinimumLength = 5)]
    string Title,

    [StringLength(500)]
    string Summary,


    [Required]

    ArticleCategory Category,

    string? Tags // Optional tags, comma-separated or JSON
);