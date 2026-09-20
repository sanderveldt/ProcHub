using System.Net;

namespace ProcHub.Wpf.Infrastructure.Api;

public sealed class ApiException : Exception
{
    public HttpStatusCode? StatusCode { get; }
    public string Title { get; }
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ApiException(
        HttpStatusCode statusCode,
        string message,
        string? title = null,
        IReadOnlyDictionary<string, 
            string[]>? errors = null)
        : base(message)
    {
        StatusCode = statusCode;

        Title = title
            ?? "request failed";

        Errors = errors ?? new
            Dictionary<string, string[]>();
    }

    public ApiException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
        StatusCode = null;
        Title = "Connection error";
        Errors = new Dictionary<string, string[]>();
    }
}