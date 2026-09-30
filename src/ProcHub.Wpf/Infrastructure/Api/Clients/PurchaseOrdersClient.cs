using System.Drawing.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Annotations;
using System.Windows.Navigation;
using ProcHub.Contracts.PurchaseOrders.Enums;
using ProcHub.Contracts.PurchaseOrders.Requests;
using ProcHub.Contracts.PurchaseOrders.Responses;

namespace ProcHub.Wpf.Infrastructure.Api.Clients;

public sealed class PurchaseOrdersApiClient(
    IHttpClientFactory httpClientFactory)
{
    public async Task<List<PurchaseOrderListItemResponse>> GetListAsync(
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .GetAsync(
                "api/purchaseorders",
                cancellationToken);
        
        var purchaseOrder = await response
            .Content.ReadFromJsonAsync<
                List<PurchaseOrderListItemResponse>>(
                    cancellationToken);
        
        return purchaseOrder
            ?? [];
    }

    public async Task<List<PurchaseOrderResponse>> GetDashboardListAsync(
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .GetAsync(
                "api/purchaseorders/dashboard",
                cancellationToken);
        
        var purchaseOrder = await response
            .Content.ReadFromJsonAsync<
                List<PurchaseOrderResponse>>(
                    cancellationToken);
        
        return purchaseOrder
            ?? [];
    }

    public async Task<PurchaseOrderResponse> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var trimmedCode = code.Trim();

        var client = CreateClient();

        using var response = await client
            .GetAsync(
                $"api/purchaseorders/{trimmedCode}",
                cancellationToken);
        
        var purchaseOrder = await response
            .Content.ReadFromJsonAsync<
                PurchaseOrderResponse>(
                cancellationToken);

        return purchaseOrder
            ?? throw new InvalidOperationException(
                "PurchaseOrder response is empty.");
    }

    public async Task<PurchaseOrderResponse> CreateAsync(
        CreatePurchaseOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .PostAsJsonAsync(
                "api/purchaseorders",
                request,
                cancellationToken);
        
        var purchaseOrder = await response
            .Content.ReadFromJsonAsync<
                PurchaseOrderResponse>(
                cancellationToken);
        
        return purchaseOrder
            ?? throw new InvalidOperationException(
                "PurchaseOrder response is empty.");
    }

    public async Task DeleteAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .DeleteAsync(
                $"api/purchaseorders/{code}",
                cancellationToken);
    }



    private HttpClient CreateClient()
    {
        return httpClientFactory
            .CreateClient(
                ApiClientNames.Authorized);
    }
    
}