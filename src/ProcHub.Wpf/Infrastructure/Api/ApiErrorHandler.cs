using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace ProcHub.Wpf.Infrastructure.Api;

public sealed class ApiErrorHandler : DelegatingHandler
{
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await base
                .SendAsync(
                    request,
                    cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            using (response)
            {
                var problemDetails =
                    await TryReadDetailsAsync(
                        response,
                        cancellationToken);

                var message = CreateMessage(
                    response.StatusCode,
                    response.ReasonPhrase,
                    problemDetails);

                throw new ApiException(
                    response.StatusCode,
                    message,
                    problemDetails?.Title,
                    problemDetails?.Errors);
            }
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException(
                "Unable to connect to the API.",
                ex);
        }
        catch (OperationCanceledException ex)
            when (!cancellationToken.IsCancellationRequested)
        {
            throw new ApiException(
                "The request to the API timed out.",
                ex);
        }
    }

    private static async Task<ApiProblemDetails?> TryReadDetailsAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var content = await response
            .Content.ReadAsStringAsync(
                cancellationToken);

        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        try
        {
            return JsonSerializer
                .Deserialize<ApiProblemDetails>(
                    content,
                    JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static string CreateMessage(
        HttpStatusCode statusCode,
        string? reasonPhrase,
        ApiProblemDetails? problemDetails)
    {
        if (problemDetails?.Errors is { Count: > 0})
        {
            var validationErrors = problemDetails.Errors
                .Values
                .SelectMany(errors => errors)
                .Where(error => !string.IsNullOrWhiteSpace(error))
                .ToArray();

            if (validationErrors.Length > 0)
            {
                return string.Join(
                    Environment.NewLine,
                    validationErrors);
            }
        }

        return problemDetails?.Detail
            ?? problemDetails?.Title
            ?? reasonPhrase
            ?? $"API request failed with status code {(int)statusCode}.";
    }

    private sealed class ApiProblemDetails
    {
        public string? Title { get; init; }

        public string? Detail { get; init; }
        public Dictionary<string, string[]>? Errors { get; init; }
    }
}