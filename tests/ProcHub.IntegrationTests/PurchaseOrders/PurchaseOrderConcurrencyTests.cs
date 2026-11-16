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
    [Fact]
    public async Task UpdateBooleans_RejectsOutdatedVersion()
    {
        using var firstClient = await factory.LoginAsync();
        using var secondClient = await factory.LoginAsync();

        var orderCode = await factory
            .CreateTestPurchaseOrderAsync(firstClient);
        var url = $"/api/purchaseorders/{orderCode}";

        var firstClientOrder = await firstClient
            .GetFromJsonAsync<PurchaseOrderResponse>(url);
        var secondClientOrder = await secondClient
            .GetFromJsonAsync<PurchaseOrderResponse>(url);

        Assert.NotNull(firstClientOrder);
        Assert.NotNull(secondClientOrder);
        Assert.Equal(
            firstClientOrder.Version,
            secondClientOrder.Version);
        

        using var firstClientUpdate = await firstClient
            .PutAsJsonAsync(
                $"{url}/booleans",
                new UpdatePurchaseOrderBooleansRequest
                {
                    InvoiceSent = true,
                    Version = firstClientOrder.Version
                });

        Assert.Equal(
            HttpStatusCode.OK, 
            firstClientUpdate.StatusCode);
        
        var updatedVersion = await firstClientUpdate
            .Content.ReadFromJsonAsync<uint>();

        Assert.NotEqual(
            firstClientOrder.Version,
            updatedVersion);
        
        // Second client is still on old version
        using var outdatedUpdate = await secondClient
            .PutAsJsonAsync(
                $"{url}/booleans",
                new UpdatePurchaseOrderBooleansRequest
                {
                    OrderInspectionDone = true,
                    Version = secondClientOrder.Version
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
}
