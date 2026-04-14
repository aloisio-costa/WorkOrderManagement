namespace WorkOrderManagement.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IReadOnlyList<string> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}