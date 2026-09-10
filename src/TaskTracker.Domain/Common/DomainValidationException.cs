namespace TaskTracker.Domain.Common;

public class DomainValidationException : Exception
{
    public string FieldName { get; }

    public DomainValidationException(string fieldName, string message) : base(message)
    {
        FieldName = fieldName;
    }
}