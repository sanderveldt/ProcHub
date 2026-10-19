using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Users.Get;
using ProcHub.Contracts.Users.Responses;

namespace ProcHub.Api.Endpoints.Users;

public static class ListUsersEndpoint
{
    public static RouteGroupBuilder MapListUsersEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/", HandleAsync)
             .WithName("ListUsers");

        return group;
    }

    private static async Task<Ok<List<UserResponse>>> HandleAsync(
        ListUsersHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler
            .HandleAsync(cancellationToken);
        
        var response = result
            .Select(u => new UserResponse(
                u.Id,
                u.UserName,
                u.Email,
                u.DisplayName,
                u.Role))
            .ToList();
        
        return TypedResults.Ok(response);
    }
}