namespace CureFusion.Contracts.Auth;

public class UserQueryParameters
{
    // Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    // Filtering
    public string? SearchTerm { get; set; }

    public string? SortBy { get; set; }
    public bool SortAscending { get; set; } = true; // Default newest first
}