using System.Net.Http;
using System.Net.Http.Json;
using ProcHub.Contracts.Authentication.Requests;
using ProcHub.Contracts.Authentication.Responses;
using ProcHub.Wpf.Infrastructure.Api;

namespace ProcHub.Wpf.Infrastructure.Api.Clients;

public sealed class AuthApiClient(
    IHttpClientFactory httpClientFactory)
{
    public async Task<TokenResponse> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory
            .CreateClient(
                ApiClientNames.Authentication);
        
        var request = new LoginRequest(
            email,
            password);
        
        using var response = await client
            .PostAsJsonAsync(
                "api/auth/login",
                request,
                cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);

        var tokenResponse = await response
            .Content.ReadFromJsonAsync<TokenResponse>(
                cancellationToken);
        
        return tokenResponse
            ?? throw new InvalidOperationException(
                "The login response contains no token data.");
    }

    public async Task<TokenResponse> RefreshAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory
            .CreateClient(
                ApiClientNames.Authentication);
        
        var request = new RefreshTokenRequest(
            refreshToken);

        using var response = await client
            .PostAsJsonAsync(
                "api/auth/refresh",
                request,
                cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);

        var tokenResponse = await response
            .Content.ReadFromJsonAsync<TokenResponse>(
                cancellationToken);
        
        return tokenResponse
            ?? throw new InvalidOperationException(
                "Refresh response contains no token data.");
    }

    public async Task<CurrentUserResponse> GetCurrentUserAsync(
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory
            .CreateClient(
                ApiClientNames.Authorized);
        
        using var response = await client
            .GetAsync(
                "api/auth/me",
                cancellationToken);
        
        await response
            .EnsureApiSuccesAsync(
                cancellationToken);

        var currentUser = await response
            .Content.ReadFromJsonAsync<CurrentUserResponse>(
                cancellationToken);
        
        return currentUser
            ?? throw new InvalidOperationException(
                "Curent user response is empty.");
    }
}