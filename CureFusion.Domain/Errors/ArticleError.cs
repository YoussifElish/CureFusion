using CureFusion.Domain.Abstactions;
using Microsoft.AspNetCore.Http;

namespace CureFusion.Domain.Errors;


public static class ArticleErrors
{
    public static readonly Error ArticleNotFound = new(
        "ArticleError.NotFound",
        "The requested article was not found.",
        StatusCodes.Status404NotFound);
    public static readonly Error ImageNotProvided = new(
        "ArticleError.ImageNotProvided",
        "The requested article Image was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error DuplicateArticleTitle = new(
        "ArticleError.DuplicateTitle",
        "An article with this title already exists.",
        StatusCodes.Status409Conflict);

    public static readonly Error InvalidArticleStatus = new(
        "ArticleError.InvalidStatus",
        "The provided article status is invalid.",
        StatusCodes.Status400BadRequest);

    public static readonly Error UnauthorizedArticleAccess = new(
        "ArticleError.UnauthorizedAccess",
        "You do not have permission to perform this action on the article.",
        StatusCodes.Status401Unauthorized);
}