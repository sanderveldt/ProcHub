using ProcHub.Contracts.Users.Requests;
using ProcHub.Contracts.Users.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Users.Update;

namespace ProcHub.Api.Endpoints.Users;

public static class UpdateUserEndpoint
{
    public static RouteGroupBuilder MapUpdateUserEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPut("/{id:int}", HandleAsync)
             .WithName("UpdateUser");
        
        return group;
    }

    private static async Task<Ok<UserResponse>> HandleAsync(
        int id,
        UpdateUserRequest request,
        UpdateUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(
            request.UserName,
            request.Role)
        {
            DisplayName = request.DisplayName   
        };

        var result = await handler.HandleAsync(
            id,
            command,
            cancellationToken);
        
        var response = new UserResponse(
            result.Id,
            result.UserName,
            result.Email,
            result.DisplayName,
            result.Role);
        
        return TypedResults.Ok(response);
    }
}