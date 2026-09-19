using System.Net;

namespace ProcHub.Wpf.Infrastructure.Api;

public sealed class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string Title { get; }
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    private static string ErrorMessage(
        HttpStatusCode statusCode,
        ApiProblemDetails? problemDetails,
        string? reasonPhrase)
    {
        return problemDetails?.Detail
            ?? problemDetails?.Title
            ?? reasonPhrase
            ?? $"API request failed with status {(int)statusCode}.";
    }

    public ApiException(
        HttpStatusCode statusCode,
        ApiProblemDetails? problemDetails,
        string? reasonPhrase = null)
        : base(
            ErrorMessage(
                statusCode,
                problemDetails,
                reasonPhrase))
    {
        StatusCode = statusCode;

        Title = problemDetails?
            .Title
            ?? "Request failed.";

        Errors = problemDetails?
            .Errors
            ?? new Dictionary<string, string[]>();
    }
}