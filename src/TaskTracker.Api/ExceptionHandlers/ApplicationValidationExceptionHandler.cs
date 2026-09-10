using Microsoft.AspNetCore.Diagnostics;
using TaskTracker.Application.Common;

namespace TaskTracker.Api.ExceptionHandlers;

public class ApplicationValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ApplicationValidationException validationException)
        {
            return false;
        }

        var result = Results.ValidationProblem(validationException.Errors);

        await result.ExecuteAsync(httpContext);
        
        return true;
    }
}