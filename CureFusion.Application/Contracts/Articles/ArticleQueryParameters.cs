using CureFusion.Domain.Entities;

namespace CureFusion.Application.Contracts.Articles;

public class ArticleQueryParameters
{
    // Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    // Filtering
    public ArticleCategory? Category { get; set; }
    public string? SearchTerm { get; set; }
    public string? Tag { get; set; }

    // Sorting
    public string SortBy { get; set; } = "PublishedDate"; // Default sort by publish date
    public bool SortDescending { get; set; } = true; // Default newest first
}