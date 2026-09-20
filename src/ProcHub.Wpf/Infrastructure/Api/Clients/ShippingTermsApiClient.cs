using System.Net.Http;
using System.Net.Http.Json;
using ProcHub.Contracts.ShippingTerms.Responses;
using ProcHub.Contracts.ShippingTerms.Requests;

namespace ProcHub.Wpf.Infrastructure.Api.Clients;
public class ShippingTermsApiClient(
    IHttpClientFactory httpClientFactory)
{        
    public async Task<List<ShippingTermResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        
        using var response = await client.GetAsync(
            "api/shipping-terms",
                cancellationToken);

        var shippingTerms = await response
            .Content.ReadFromJsonAsync<
                List<ShippingTermResponse>>(
                cancellationToken);

        return shippingTerms 
            ?? [];
    }

    public async Task<ShippingTermResponse> GetbyIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client.GetAsync(
            $"api/shipping-terms/{id}",
            cancellationToken);

        var shippingTerm = await response
            .Content.ReadFromJsonAsync<
                ShippingTermResponse>(
                cancellationToken);

        return shippingTerm
            ?? throw new InvalidOperationException(
                "ShippingTerm response is empty.");
    }

    public async Task<ShippingTermResponse> CreateAsync(
        CreateShippingTermRequest request,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .PostAsJsonAsync(
                "api/shipping-terms",
                request,
                cancellationToken);

        var shippingTerm = await response
            .Content.ReadFromJsonAsync<
                ShippingTermResponse>(
                cancellationToken);

        return shippingTerm
            ?? throw new InvalidOperationException(
                "Shipping Term response is empty.");
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .DeleteAsync(
                $"api/shipping-terms/{id}",
                cancellationToken);
    }
        
    private HttpClient CreateClient()
    {
        return httpClientFactory
            .CreateClient(
            ApiClientNames.Authorized);
    }
}

