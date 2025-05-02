using CureFusion.Abstactions;
using CureFusion.Contracts.Articles;
using CureFusion.Contracts.Files;
using CureFusion.Entities;
using CureFusion.Errors;
using CureFusion.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RealState.Services;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Linq.Dynamic.Core;
using static System.Net.WebRequestMethods;
namespace CureFusion.Services;

public class ArticleService(ApplicationDbContext context, IFileService fileService, IWebHostEnvironment webHostEnvironment) : IArticleService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IFileService _fileService = fileService;
    private readonly string _filesPath = $"https://curefusion2.runasp.net/Uploads";

    public async Task<Result<ArticleResponse>> CreateArticleAsync(CreateArticleRequest request, string Content, UploadImageRequest? articleImage, string authorId, CancellationToken cancellationToken)
    {
    

        Guid? imageId = null;
        if (articleImage?.Image is  null)
        {
            return Result.Failure<ArticleResponse>(ArticleErrors.ImageNotProvided);
        }
        var imageResult = await _fileService.UploadImagesAsync(articleImage.Image, cancellationToken);

        imageId = imageResult.Id;
        var article = new HealthArticle
        {
            Title = request.Title,
            Summary = request.Summary,
            Content = Content,
            Category = request.Category,
            Tags = request.Tags ?? string.Empty,
            Status = ArticleStatus.Published, 
            PublishedDate = DateTime.UtcNow,
            AuthorId = authorId,
            HealthArticleImageId = imageId
        };

        await _context.HealthArticles.AddAsync(article, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);


        var response = article.Adapt<ArticleResponse>();
        response = response with { ImageUrl = $"{_filesPath}/{imageResult.StoredFileName}" };




        return Result.Success(response);
    }

    public async Task<Result> DeleteArticleAsync(int id, CancellationToken cancellationToken)
    {
        var article = await _context.HealthArticles.FindAsync( id , cancellationToken);
        if (article is null)
        {
            return Result.Failure(ArticleErrors.ArticleNotFound);
        }


        if (article.HealthArticleImageId.HasValue)
        {
           var images = await _context.UploadedFiles.Where(x=> x.Id == article.HealthArticleImageId).ToListAsync(cancellationToken);
            if (images.Any())
            {
                 _context.RemoveRange(images);
              
            }
        }

        _context.HealthArticles.Remove(article);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<ArticleResponse>>> GetAllArticlesAsync(ArticleQueryParameters queryParams, CancellationToken cancellationToken)
    {
        var query = _context.HealthArticles
            .Include(a => a.Author)
            .Include(a => a.HealthArticleImage) 
            .AsNoTracking();

        // Filtering
        if (queryParams.Category.HasValue)
        {
            query = query.Where(a => a.Category == queryParams.Category );
        }
        if (!string.IsNullOrWhiteSpace(queryParams.Tag))
        {
            // Assuming Tags is comma-separated
            query = query.Where(a => a.Tags.ToLower().Contains(queryParams.Tag.ToLower()));
        }
  
        if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
        {
            query = query.Where(a => a.Title.ToLower().Contains(queryParams.SearchTerm.ToLower()) ||
                                     a.Summary.ToLower().Contains(queryParams.SearchTerm.ToLower()) ||
                                     a.Content.ToLower().Contains(queryParams.SearchTerm.ToLower()));
        }

        // Sorting
        if (!string.IsNullOrWhiteSpace(queryParams.SortBy))
        {
            var sortColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    { "title", "Title" },
    { "viewcount", "ViewCount" },
    { "publisheddate", "PublishedDate" }
};
            sortColumns.TryGetValue(queryParams.SortBy, out var sortBy);

            var sorting = $"{sortBy} {(queryParams.SortDescending ? "descending" : "ascending")}";
            query = query.OrderBy(sorting);
        }
        else
        {
            query = query.OrderByDescending(a => a.PublishedDate); // Default sorting
        }

        // Pagination
        var totalItems = await query.CountAsync(cancellationToken);
        var articles = await query
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync(cancellationToken);

        // Adapt to Response DTOs
        var response = articles.Select(article => new ArticleResponse(
            article.Id,
            article.Title,
            article.Summary,
            article.Content, 
            article.Category,
            article.Tags,
            article.Status.ToString(),
            article.PublishedDate,
            article.ViewCount,
            article.Author != null ? $"{article.Author.FirstName} {article.Author.LastName}" : "Unknown",
           article.Author != null ? $"{article.Author.Id}" : "Unknown",
            article.HealthArticleImage != null ? $"{_filesPath}/{article.HealthArticleImage.StoredFileName}" : null 
        )).ToList();

   
        return Result.Success<IEnumerable<ArticleResponse>>(response);
    }

    public async Task<Result<ArticleResponse>> GetArticleByIdAsync(int id, CancellationToken cancellationToken)
    {
        var article = await _context.HealthArticles
            .Include(a => a.Author)
            .Include(a => a.HealthArticleImage)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (article is null)
        {
            return Result.Failure<ArticleResponse>(ArticleErrors.ArticleNotFound);
        }

        var response = new ArticleResponse(
             article.Id,
             article.Title,
             article.Summary,
             article.Content,
             article.Category,
             article.Tags,
             article.Status.ToString(),
             article.PublishedDate,
             article.ViewCount,
             article.Author?.UserName ?? "Unknown",
             article.Author != null ? $"{article.Author.Id}" : "Unknown",
             article.HealthArticleImage != null ? $"{_filesPath}/{article.HealthArticleImage.StoredFileName}" : null
         );
        await IncrementArticleViewCountAsync(response.Id, cancellationToken);
        return Result.Success(response);
    }

  
    public async Task<Result> IncrementArticleViewCountAsync(int id, CancellationToken cancellationToken)
    {
        var article = await _context.HealthArticles.FindAsync( id , cancellationToken);
        if (article is null)
        {
            return Result.Failure(ArticleErrors.ArticleNotFound);
        }

        article.ViewCount++;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateArticleAsync(int id, string content,UpdateArticleRequest request, UploadImageRequest? articleImage, CancellationToken cancellationToken)
    {
        var article = await _context.HealthArticles.FindAsync( id , cancellationToken);
        if (article is null)
        {
            return Result.Failure(ArticleErrors.ArticleNotFound);
        }

        article.Title = request.Title;
        article.Summary = request.Summary;
        article.Content = content;
        article.Category = request.Category;
        article.Tags = request.Tags ?? string.Empty;


        if (articleImage?.Image is not null)
        {
           
            if (article.HealthArticleImageId.HasValue)
            {
                var images = await _context.UploadedFiles.Where(x => x.Id == article.HealthArticleImageId).ToListAsync(cancellationToken);
                if (images.Any())
                {
                    _context.RemoveRange(images);

                }
            }
         
            var imageResult = await _fileService.UploadImagesAsync(articleImage.Image, cancellationToken);
            article.HealthArticleImageId = imageResult.Id;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UpdateArticleStatusAsync(int id, UpdateArticleStatusRequest request, CancellationToken cancellationToken)
    {
        var article = await _context.HealthArticles.FindAsync( id , cancellationToken);
        if (article is null)
        {
            return Result.Failure(ArticleErrors.ArticleNotFound);
        }

        if (Enum.TryParse<ArticleStatus>(request.Status, true, out var newStatus))
        {
            article.Status = newStatus;
           
            if (newStatus == ArticleStatus.Published && article.Status != ArticleStatus.Published) 
            {
                article.PublishedDate = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        else
        {
            return Result.Failure(ArticleErrors.InvalidArticleStatus);
        }
    }

  
  
}

