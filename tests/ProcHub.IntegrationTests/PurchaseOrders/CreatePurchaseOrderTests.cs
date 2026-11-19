using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using ProcHub.Contracts.PurchaseOrders.Enums;
using ProcHub.Contracts.PurchaseOrders.Requests;
using ProcHub.Contracts.PurchaseOrders.Responses;
using ProcHub.Contracts.Suppliers.Requests;
using ProcHub.Contracts.Suppliers.Responses;
using ProcHub.IntegrationTests.Fixtures;
using ProcHub.IntegrationTests.Helpers;

namespace ProcHub.IntegrationTests.PurchaseOrders;

public sealed class CreatePurchaseOrderTests(ProcHubApiFactory factory)
    : IClassFixture<ProcHubApiFactory>
{
    [Fact]
    public async Task RegularOrder_UsesSupplierTerms_AndIsPersisted()
    {
        // Arrange: look up seeded terms
        var (paymentTermId, shippingTermId) = await factory.InDbAsync(async db =>
        {
            var paymentTermId = await db.PaymentTerms
                .Where(term => term.Description == "30 days after invoice.")
                .Select(term => term.Id)
                .SingleAsync();

            var shippingTermId = await db.ShippingTerms
                .Where(term => term.Name == "EXW")
                .Select(term => term.Id)
                .SingleAsync();

            return (paymentTermId, shippingTermId);
        });

        using var client = await factory.LoginAsync();
        var supplierCode = ApiTestHelpers.NewCode("SUP");
        var orderCode = ApiTestHelpers.NewCode("PO3");

        var supplierRequest = new CreateSupplierRequest(
            supplierCode,
            "Test supplier",
            paymentTermId,
            shippingTermId)
        {
            MainLeadTimeDays = 32,
            ProductionTimeDays = 30,
            ShippingTimeDays = 12
        };

        // Act: create supplier through the API.
        var supplierResponse = await client
            .PostAsJsonAsync(
                "/api/suppliers/", 
                supplierRequest);

        Assert.Equal(
            HttpStatusCode.Created, 
            supplierResponse.StatusCode);

        var supplier = await supplierResponse.Content
            .ReadFromJsonAsync<SupplierResponse>();

        Assert.NotNull(supplier);
        Assert.Equal(
            supplierCode, 
            supplier.Code);

        // The supplier response has no ID, so read it from the test database.
        var supplierId = await factory.InDbAsync(db => db.Suppliers
            .Where(item => item.Code == supplierCode)
            .Select(item => item.Id)
            .SingleAsync());

        var orderRequest = new CreatePurchaseOrderRequest(
            orderCode,
            PurchaseOrderType.Regular,
            supplierId);

        var createResponse = await client.PostAsJsonAsync(
            "/api/purchaseorders/", orderRequest);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.Equal($"/api/purchaseorders/{orderCode}",
            createResponse.Headers.Location?.ToString());

        var createdOrder = await createResponse.Content
            .ReadFromJsonAsync<CreatePurchaseOrderResponse>();
        Assert.NotNull(createdOrder);
        Assert.Equal("30 days after invoice.",
            createdOrder.PaymentTermDescription);
        Assert.Equal("EXW", createdOrder.ShippingTermName);
        Assert.Equal(32, createdOrder.LeadTimeDays);

        // Assert: a GET request to check the created order
        var getResponse = await client.GetAsync(
            $"/api/purchaseorders/{orderCode}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetchedOrder = await getResponse.Content
            .ReadFromJsonAsync<PurchaseOrderResponse>();
        Assert.NotNull(fetchedOrder);
        Assert.Equal(
            orderCode, 
            fetchedOrder.Code);
        Assert.Equal(
            supplierCode, 
            fetchedOrder.SupplierCode);
        Assert.Equal(
            paymentTermId, 
            fetchedOrder.PaymentTermId);
        Assert.Equal(
            shippingTermId, 
            fetchedOrder.ShippingTermId);
        Assert.Equal(
            30, 
            fetchedOrder.BalanceDueDays);
        Assert.Equal(
            32, 
            fetchedOrder.LeadTimeDays);

        // Check stored snapshot
        var savedOrder = await factory.ReadOrderAsync(orderCode);
        Assert.Equal(
            supplierCode, 
            savedOrder.SupplierCode);
        Assert.Equal(
            "30 days after invoice.",
            savedOrder.PaymentTermDescription);
        Assert.Equal(
            "EXW", 
            savedOrder.ShippingTermName);
        Assert.Equal(
            32, 
            savedOrder.LeadTimeDays);
    }

    [Fact]
    public async Task CreatePurchaseOrder_WithExistingCode_ReturnsConflict()
    {
        using var client = await factory.LoginAsync();

        var orderCode = await factory
            .CreateTestPurchaseOrderAsync(client);
        var testOrder = await factory
            .ReadOrderAsync(orderCode);
        
        using var duplicateOrder = await client
            .PostAsJsonAsync(
                "api/purchaseorders/",
                new CreatePurchaseOrderRequest(
                    testOrder.Code,
                    PurchaseOrderType.Regular,
                    testOrder.SupplierId));
        
        Assert.Equal(
            HttpStatusCode.Conflict, 
            duplicateOrder.StatusCode);
        
        var orderCodeCount = await factory
            .InDbAsync(db => db.PurchaseOrders
                .CountAsync(
                    po => po.Code == testOrder.Code));
        
        Assert.Equal(1, orderCodeCount);   
    }
}
