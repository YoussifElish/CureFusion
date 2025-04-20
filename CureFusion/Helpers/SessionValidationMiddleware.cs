namespace CureFusion.Helpers;

using CureFusion.Abstactions;
using CureFusion.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CureFusion.Persistence;
public class SessionValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public SessionValidationMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context, ISessionService sessionService)
    {
        var endpoint = context.GetEndpoint();

        var hasAuthorizeAttribute = endpoint?.Metadata?.GetMetadata<IAuthorizeData>() != null;
        if (!hasAuthorizeAttribute)
        {
            await _next(context);
            return;
        }

        var token = context.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

        if (string.IsNullOrWhiteSpace(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Authorization token is missing.");
            return;
        }

        var session = await sessionService.IsSessionValidAsync(token, context.RequestAborted);
        if (session == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid or expired session token.");
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userExists = await dbContext.Users.AnyAsync(u => u.Id == session.UserId, context.RequestAborted);
        if (!userExists)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("User not found.");
            return;
        }

        await _next(context);
    }
}