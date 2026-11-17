using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using ProcHub.Contracts.PurchaseOrders.Requests;
using ProcHub.Contracts.PurchaseOrders.Responses;
using ProcHub.IntegrationTests.Fixtures;
using ProcHub.IntegrationTests.Helpers;

namespace ProcHub.IntegrationTests.PurchaseOrders;

public sealed class PurchaseOrderConcurrencyTests(
    ProcHubApiFactory factory)
    : IClassFixture<ProcHubApiFactory>
{
    private async Task<(string Url, uint Version)> CreateSharedOrderAsync(
        HttpClient firstClient,
        HttpClient secondClient)
{
    var orderCode = await factory.CreateTestPurchaseOrderAsync(firstClient);
    var url = $"/api/purchaseorders/{orderCode}";

    var firstOrder = await firstClient
        .GetFromJsonAsync<PurchaseOrderResponse>(url);
    var secondOrder = await secondClient
        .GetFromJsonAsync<PurchaseOrderResponse>(url);

    Assert.NotNull(firstOrder);
    Assert.NotNull(secondOrder);
    Assert.Equal(firstOrder.Version, secondOrder.Version);

    return (url, firstOrder.Version);
}

    [Fact]
    public async Task UpdateBooleans_RejectsOutdatedVersion()
    {
        using var firstClient = await factory.LoginAsync();
        using var secondClient = await factory.LoginAsync();

        var (url, version) = await 
            CreateSharedOrderAsync(
                firstClient, 
                secondClient);
        
        using var firstClientUpdate = await firstClient
            .PutAsJsonAsync(
                $"{url}/booleans",
                new UpdatePurchaseOrderBooleansRequest
                {
                    InvoiceSent = true,
                    Version = version
                });

        Assert.Equal(
            HttpStatusCode.OK, 
            firstClientUpdate.StatusCode);
        
        var updatedVersion = await firstClientUpdate
            .Content.ReadFromJsonAsync<uint>();

        Assert.NotEqual(
            version,
            updatedVersion);
        
        // Second client is still on old version
        using var outdatedUpdate = await secondClient
            .PutAsJsonAsync(
                $"{url}/booleans",
                new UpdatePurchaseOrderBooleansRequest
                {
                    OrderInspectionDone = true,
                    Version = version
                });
        
        // Global exception handler returns Conflict for ConcurrencyExceptions
        Assert.Equal(
            HttpStatusCode.Conflict,
            outdatedUpdate.StatusCode);
        
        // Check if firstClientUpdate wasnt overwritten
        var updatedOrder = await firstClient
            .GetFromJsonAsync<PurchaseOrderResponse>(url);
        
        Assert.NotNull(updatedOrder);
        Assert.True(updatedOrder.InvoiceSent);
        Assert.False(updatedOrder.InspectionDone);
        Assert.Equal(updatedVersion, updatedOrder.Version);
    }

    [Fact]
    public async Task UpdateDates_RejectsOutdatedVersion()
    {
        using var firstClient = await factory.LoginAsync();
        using var secondClient = await factory.LoginAsync();

        var (url, version) = await
            CreateSharedOrderAsync(
                firstClient,
                secondClient);
        
        var prodDoneDate = ApiTestHelpers.CreateTestDate();
        
        using var firstClientUpdate = await firstClient
            .PutAsJsonAsync(
                $"{url}/dates",
                new UpdatePurchaseOrderDatesRequest
                {
                    ProductionDoneDate = prodDoneDate,
                    Version = version
                });
            
        Assert.Equal(
            HttpStatusCode.OK, 
            firstClientUpdate.StatusCode);
        
        var updatedVersion = await firstClientUpdate
            .Content.ReadFromJsonAsync<uint>();
        
        Assert.NotEqual(
            version,
            updatedVersion);
        
        using var outdatedUpdate = await secondClient
            .PutAsJsonAsync(
                $"{url}/dates",
                new UpdatePurchaseOrderDatesRequest
                {
                    ShippingDate = ApiTestHelpers
                        .CreateTestDate(),
                    
                    Version = version
                });
        
        Assert.Equal(
            HttpStatusCode.Conflict,
            outdatedUpdate.StatusCode);
        
        var updatedOrder = await firstClient
            .GetFromJsonAsync<PurchaseOrderResponse>(url);
        
        Assert.NotNull(updatedOrder);
        Assert.NotNull(updatedOrder.ProductionDoneDate);
        Assert.Null(updatedOrder.ShippingDate);
        Assert.Equal(updatedVersion, updatedOrder.Version);
        Assert.Equal(
            prodDoneDate, 
            updatedOrder.ProductionDoneDate);
    }


}
