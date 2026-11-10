using System.Net.Http;
using System.Net.Http.Json;
using ProcHub.Contracts.Users.Requests;
using ProcHub.Contracts.Users.Responses;

namespace ProcHub.Wpf.Infrastructure.Api.Clients;

public sealed class UsersApiClient(
    IHttpClientFactory httpClientFactory)
{
    public async Task<List<UserResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .GetAsync(
                "api/users",
                cancellationToken);

        var users = await response
            .Content.ReadFromJsonAsync<
                List<UserResponse>>(
                cancellationToken);

        return users
            ?? [];
    }

    public async Task<UserResponse> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .GetAsync(
                $"api/users/{id}",
                cancellationToken);

        var user = await response
            .Content.ReadFromJsonAsync<
                UserResponse>(
                cancellationToken);

        return user
            ?? throw new InvalidOperationException(
                "User response is empty.");
    }

    public async Task<UserResponse> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .PostAsJsonAsync(
                "api/users",
                request,
                cancellationToken);

        var user = await response
            .Content.ReadFromJsonAsync<
                UserResponse>(
                cancellationToken);

        return user
            ?? throw new InvalidOperationException(
                "User response is empty.");
    }

    public async Task<UserResponse> UpdateAsync(
        int id,
        UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .PutAsJsonAsync(
                $"api/users/{id}",
                request,
                cancellationToken);

        var user = await response
            .Content.ReadFromJsonAsync<
                UserResponse>(
                cancellationToken);

        return user
            ?? throw new InvalidOperationException(
                "User response is empty.");
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var client = CreateClient();

        using var response = await client
            .DeleteAsync(
                $"api/users/{id}",
                cancellationToken);
    }

    private HttpClient CreateClient()
    {
        return httpClientFactory
            .CreateClient(
            ApiClientNames.Authorized);
    }
}