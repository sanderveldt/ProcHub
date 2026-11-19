using System.Net;
using System.Net.Http.Json;
using ProcHub.Application.Authorization;
using ProcHub.Contracts.Users.Requests;
using ProcHub.IntegrationTests.Fixtures;
using ProcHub.IntegrationTests.Helpers;

namespace ProcHub.IntegrationTests.PurchaseOrders;

public sealed class PurchaseOrderAuthorizationTests(
    ProcHubApiFactory factory)
    : IClassFixture<ProcHubApiFactory>
{
    [Fact]
    public async Task ListPurchaseOrders_WithoutToken_ReturnsUnauthorized()
    {
        // no login
        using var client = factory.CreateClient(); 
        
        using var response = await client
            .GetAsync("/api/purchaseorders/");

        Assert.Equal(
            HttpStatusCode.Unauthorized, 
            response.StatusCode);
    }

    [Fact]
    public async Task DeletePurchaseOrder_AsNormalUser_ReturnForbidden()
    {
        using var adminClient = await factory.LoginAsync();
        string orderCode = await factory
            .CreateTestPurchaseOrderAsync(adminClient);
        var url = $"/api/purchaseorders/{orderCode}";

        string email = "testEmail@integration.test";
        string password = "TestPassword32!";

        using var createUser = await adminClient
            .PostAsJsonAsync(
                "/api/users/",
                new CreateUserRequest(
                    email,
                    password,
                    AppRoles.User));

        Assert.Equal(
            HttpStatusCode.Created,
            createUser.StatusCode);
        
        using var regularUser = await factory
            .LoginAsync(email, password);
        
        using var deleteOrder = await regularUser
            .DeleteAsync(url);
        
        Assert.Equal(
            HttpStatusCode.Forbidden, 
            deleteOrder.StatusCode);
        
        using var getOrder = await adminClient
            .GetAsync(url);
        
        Assert.Equal(
            HttpStatusCode.OK,
            getOrder.StatusCode);
    }
}