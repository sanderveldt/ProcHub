using System.Net.Http;
using System.Net.Http.Json;
using ProcHub.Contracts.Suppliers.Requests;
using ProcHub.Contracts.Suppliers.Responses;

namespace ProcHub.Wpf.Infrastructure.Api.Clients;

public sealed class SuppliersApiClient(
    IHttpClientFactory httpClientFactory)
{
    public async Task<List<SupplierListItemResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .GetAsync(
                "api/suppliers",
                cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);

        var supplier = await response
            .Content.ReadFromJsonAsync<
                List<SupplierListItemResponse>>(
                cancellationToken);
                
        return supplier
            ?? [];        
    }

    public async Task<SupplierResponse> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var trimmedCode = code.Trim();

        var client = CreateClient();

        using var response = await client
            .GetAsync(
                $"api/suppliers/{trimmedCode}",
                cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);

        var supplier = await response
            .Content.ReadFromJsonAsync<
                SupplierResponse>(
                cancellationToken);

        return supplier
            ?? throw new InvalidOperationException(
                "Supplier response is empty.");                
    }

    public async Task<SupplierResponse> CreateAsync(
        CreateSupplierRequest request,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .PostAsJsonAsync(
                "api/suppliers",
                request,
                cancellationToken);

        await response
            .EnsureApiSuccesAsync(
                cancellationToken);

        var supplier = await response
            .Content.ReadFromJsonAsync<
                SupplierResponse>(
                cancellationToken);
        
        return supplier
            ?? throw new InvalidOperationException(
                "Supplier response is empty.");
    }

    public async Task<SupplierResponse> UpdateAsync(
        string code,
        UpdateSupplierRequest request,
        CancellationToken cancellationToken = default)
    {
        var trimmedCode = code.Trim();

        var client = CreateClient();

        using var response = await client
            .PutAsJsonAsync(
                $"api/suppliers/{trimmedCode}",
                request,
                cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);

        var supplier = await response
            .Content.ReadFromJsonAsync<
                SupplierResponse>(
                cancellationToken);
        
        return supplier
            ?? throw new InvalidOperationException(
                "Supplier response is empty.");
    }

    public async Task<ChangeSupplierStatusResponse> ChangeStatusAsync(
        string code,
        ChangeSupplierStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var trimmedCode = code.Trim();

        var client = CreateClient();

        using var response = await client
            .PatchAsJsonAsync(
                $"api/suppliers/{trimmedCode}/status",
                request,
                cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);
        
        var supplier = await response
            .Content.ReadFromJsonAsync<
                ChangeSupplierStatusResponse>(
                cancellationToken);
        
        return supplier
            ?? throw new InvalidOperationException(
                "Change Supplier reponse is empty.");
    }

    private HttpClient CreateClient()
    {
        return httpClientFactory
            .CreateClient(
                ApiClientNames.Authorized);
    }
}