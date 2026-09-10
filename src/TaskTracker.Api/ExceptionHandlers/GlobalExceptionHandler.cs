using Microsoft.AspNetCore.Diagnostics;

namespace TaskTracker.Api.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception, 
            "Unhandled exception occured while processing HTTP request {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path);

        var result = Results.Problem(
            title: "Internal server error",
            detail: "An unexpected error occured",
            statusCode: StatusCodes.Status500InternalServerError
        );
        
        await result.ExecuteAsync(httpContext);
        
        return true;
    }
}