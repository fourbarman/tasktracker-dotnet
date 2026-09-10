using Microsoft.AspNetCore.Diagnostics;
using TaskTracker.Domain.Common;

namespace TaskTracker.Api.ExceptionHandlers;

public class DomainValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not DomainValidationException domainException)
        {
            return false;
        }
        
        var result = Results.ValidationProblem(new Dictionary<string, string[]>
        {
            [domainException.FieldName] = [domainException.Message]
        });

        await result.ExecuteAsync(httpContext);

        return true;
    }
}