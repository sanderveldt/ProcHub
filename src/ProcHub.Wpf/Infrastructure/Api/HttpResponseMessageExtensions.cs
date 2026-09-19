using System.Net.Http;
using System.Security.Policy;
using System.Text.Json;

namespace ProcHub.Wpf.Infrastructure.Api;

public static class HttpResponseMessageExtensions
{
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);
    
    public static async Task EnsureApiSuccesAsync(
        this HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        ApiProblemDetails? problemDetails = null;

        var content = await response
            .Content.ReadAsStringAsync(
                cancellationToken);

        if (!string.IsNullOrWhiteSpace(content))
        {
            try
            {
                problemDetails = JsonSerializer
                    .Deserialize<ApiProblemDetails>(
                        content,
                        JsonOptions);
            }
            catch (JsonException)
            {
            }
        }

        throw new ApiException(
            response.StatusCode,
            problemDetails,
            response.ReasonPhrase);
    }
}