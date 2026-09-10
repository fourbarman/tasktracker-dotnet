namespace TaskTracker.Application.Common;

public class ApplicationValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ApplicationValidationException(IDictionary<string, string[]> errors) 
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}