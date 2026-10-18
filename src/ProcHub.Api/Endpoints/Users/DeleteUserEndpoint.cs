using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Authorization;
using ProcHub.Application.Features.Users.Delete;

namespace ProcHub.Api.Endpoints.Users;

public static class DeleteUserEndpoint
{
    public static RouteGroupBuilder MapDeleteUserEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", HandleAsync)
             .WithName("DeleteUser")
             .RequireAuthorization(
                AppPolicies.AdminOnly);
        
        return group;
    }

    private static async Task<NoContent> HandleAsync(
        int id,
        DeleteUserHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(id, cancellationToken);

        return TypedResults.NoContent();
    }
}