namespace CureFusion.Abstactions;

public class PageinatedList<T>(List<T> items, int pageNumber, int count, int pagesize)
{
    public List<T> Items { get; private set; } = items;
    public int PageNumber { get; private set; } = pageNumber;
    public int TotalPages { get; private set; } = (int)Math.Ceiling(count / (double)pagesize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;


    public static async Task<PageinatedList<T>> CreateAsync(IQueryable<T> source, int pagenumber, int pagesize,CancellationToken cancellationToken)
    { 
        var count= await source.CountAsync(cancellationToken);
        var items = await source.Skip((pagenumber - 1) * pagesize).Take(pagesize).ToListAsync(cancellationToken);
        return new PageinatedList<T>(items, pagenumber, count, pagesize);
    }

}

