using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using ProcHub.Contracts.Authentication.Requests;
using ProcHub.Contracts.Authentication.Responses;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.Suppliers;
using ProcHub.Infrastructure.Persistence;
using ProcHub.IntegrationTests.Fixtures;

namespace ProcHub.IntegrationTests.Helpers;

public static class ApiTestHelpers
{
    public static async Task<HttpClient> LoginAsync(
        this ProcHubApiFactory factory,
        string email = ProcHubApiFactory.AdminEmail,
        string password = ProcHubApiFactory.AdminPassword)
    {
        var client = factory.CreateClient();

        try
        {
            var response = await client
                .PostAsJsonAsync(
                    "api/auth/login",
                    new LoginRequest(email, password));
            
            Assert.True(
                response.StatusCode == HttpStatusCode.OK,
                $"Login failed: {(int)response.StatusCode} " +
                $" {await response.Content.ReadAsStringAsync()}");


            var tokens = await response.Content
                .ReadFromJsonAsync<TokenResponse>();
            
            Assert.NotNull(tokens);
            Assert.False(
                string.IsNullOrWhiteSpace(
                    tokens.AccessToken));
            

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    tokens.AccessToken);
            
            return client;
        }
        catch
        {
            client.Dispose();
            throw;
        }
    }
    
    // For db operations that dont return value
    public static async Task InDbAsync(
        this ProcHubApiFactory factory,
        Func<ProcHubContext, Task> operation)
    {
        await using var scope = factory
            .Services.CreateAsyncScope();
        
        var dbContext = scope.ServiceProvider
            .GetRequiredService<ProcHubContext>();
        
        await operation(dbContext);
    }

    // For db operations that do return a value
    public static async Task<T> InDbAsync<T>(
        this ProcHubApiFactory factory,
        Func<ProcHubContext, Task<T>> operation)
    {
        await using var scope = factory
            .Services.CreateAsyncScope();
        
        var dbContext = scope.ServiceProvider
            .GetRequiredService<ProcHubContext>();
        
        return await operation(dbContext);
    }

    // Generates unique Code, 10 chars
    public static string NewCode(string prefix) =>
        prefix + Guid.NewGuid().ToString("N")[..7];
    

    public static async Task<PurchaseOrder> ReadOrderAsync(
        this ProcHubApiFactory factory,
        string code)
    {
        return await factory
            .InDbAsync(db => db.PurchaseOrders
                .AsNoTracking()
                .SingleAsync(po => po.Code == code));
    }



    
}