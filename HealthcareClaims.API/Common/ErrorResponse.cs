namespace HealthcareClaims.API.Common;

public class ErrorResponse
{
    public int StatusCode { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public string TraceId { get; init; } = string.Empty;

    public IEnumerable<string>? Errors { get; init; }
}