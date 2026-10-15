using ProcHub.Contracts.Users.Requests;
using ProcHub.Contracts.Users.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Users.Create;

namespace ProcHub.Api.Endpoints.Users;

public static class CreateUserEndpoint
{
    public static RouteGroupBuilder MapCreateUserEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync);

        return group;
    }

    private static async Task<Created<UserResponse>> HandleAsync(
        CreateUserRequest request,
        CreateUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(
            request.Email,
            request.Password,
            request.Role)
        {
            DisplayName = request.DisplayName
        };

        var result = await handler.HandleAsync(
            command,
            cancellationToken);
        
        var response = new UserResponse(
            result.Id,
            result.Email,
            result.DisplayName,
            result.Role);
        
        return TypedResults.Created(
            $"/api/users/{response.Id}",
            response);   
    }
}