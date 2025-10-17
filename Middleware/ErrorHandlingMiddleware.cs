using BookManagement.Common;

namespace BookManagement.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> log)
{
    public async Task Invoke(HttpContext ctx)
    {
        try { await next(ctx); }
        catch (DomainException ex)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            await ctx.Response.WriteAsJsonAsync(new { error = ex.Message, traceId = ctx.TraceIdentifier });
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unhandled error");
            ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await ctx.Response.WriteAsJsonAsync(new { error = "Unexpected error", traceId = ctx.TraceIdentifier });
        }
    }
}