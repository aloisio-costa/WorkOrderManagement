namespace WorkOrderManagement.Api.Contracts.Common;

public class ErrorResponse
{
    public int StatusCode { get; init; }
    public string Message { get; init; } = string.Empty;
    public IReadOnlyList<string>? Errors { get; init; }
}