namespace CureFusion.Application.Contracts.Articles;

public class DrugQueryParameters
{
    // Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    // Filtering
    public string? SearchTerm { get; set; }

}