using System.Net.Http;
using System.Net.Http.Json;
using ProcHub.Contracts.PaymentTerms.Requests;
using ProcHub.Contracts.PaymentTerms.Responses;

namespace ProcHub.Wpf.Infrastructure.Api.Clients;

public sealed class PaymentTermsApiClient(
    IHttpClientFactory httpClientFactory)
{
    public async Task<List<PaymentTermListItemResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client.GetAsync(
            "api/payment-terms",
                cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);

        var paymentTerms = await response
            .Content.ReadFromJsonAsync<
                List<PaymentTermListItemResponse>>(
                cancellationToken);
        
        return paymentTerms
            ?? [];
    }

    public async Task<PaymentTermResponse> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client.GetAsync(
            $"api/payment-terms/{id}",
            cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);

        var paymentTerm = await response
            .Content.ReadFromJsonAsync<
                PaymentTermResponse>(
                cancellationToken);
        
        return paymentTerm
            ?? throw new InvalidOperationException(
                "PaymentTerm response is empty.");
    }

    public async Task<PaymentTermResponse> CreateAsync(
        CreatePaymentTermRequest request,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .PostAsJsonAsync(
                "api/payment-terms",
                request,
                cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);

        var paymentTerm = await response
            .Content.ReadFromJsonAsync<
                PaymentTermResponse>(
                cancellationToken);
        
        return paymentTerm
            ?? throw new InvalidOperationException(
                "PaymentTerm response is empty.");
    }

    public async Task<PaymentTermResponse> UpdateAsync(
        int id,
        UpdatePaymentTermRequest request,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .PutAsJsonAsync(
                $"api/payment-terms/{id}",
                request,
                cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);

        var paymentTerm = await response
            .Content.ReadFromJsonAsync<
                PaymentTermResponse>(
                cancellationToken);
                
        return paymentTerm
            ?? throw new InvalidOperationException(
                "PaymentTerm response is empty.");
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .DeleteAsync(
                $"api/payment-terms/{id}",
                cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);
    }

    private HttpClient CreateClient()
    {
        return httpClientFactory
            .CreateClient(
                ApiClientNames.Authorized);
    }
}