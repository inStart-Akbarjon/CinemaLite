using CinemaLite.Api.Contracts.Common;
using CinemaLite.Application.Exceptions.Abstract;

namespace CinemaLite.Api.Middlewares;

public sealed class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (AppException ex)
        {
            await HandleAppException(ex, httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            await HandleException(ex, httpContext);
        }
    }

    private static async Task HandleAppException(AppException ex, HttpContext httpContext)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = ex.StatusCode;

        var problemDetails = new ApiErrorResponse()
        {
            Title = ex.ErrorCode,
            Status = ex.StatusCode,
            Details = ex.Message,
        };
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails);
    }

    private static async Task HandleException(Exception ex, HttpContext httpContext)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        var problemDetails = new ApiErrorResponse()
        {
            Title = ex.GetType().Name,
            Status = StatusCodes.Status500InternalServerError,
            Details = ex.Message
        };
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails);
    }
}