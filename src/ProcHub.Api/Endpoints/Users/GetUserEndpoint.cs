using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using ProcHub.Infrastructure.Identity;
using ProcHub.Contracts.Users.Responses;
using ProcHub.Application.Features.Users.Get;

namespace ProcHub.Api.Endpoints.Users;

public static class GetUserEndpoint
{
    public static RouteGroupBuilder MapGetUserEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", HandleAsync)
             .WithName("GetUser");

        return group;    
    }

    private static async Task<Ok<UserResponse>> HandleAsync(
        int id,
        GetUserHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetUserQuery(id);

        var result = await handler.HandleAsync(
            query,
            cancellationToken);
        
        var response = new UserResponse(
            result.Id,
            result.Email,
            result.DisplayName,
            result.Role);
        
        return TypedResults.Ok(response);
    }
}

