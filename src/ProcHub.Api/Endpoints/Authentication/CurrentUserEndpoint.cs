using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Abstractions;
using ProcHub.Contracts.Authentication.Responses;

namespace ProcHub.Api.Endpoints.Authentication;

public static class CurrentUserEndpoint
{
    public static RouteGroupBuilder MapCurrentUserEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/me", HandleAsync)
             .RequireAuthorization()
             .WithName("CurrentUser");

        return group;
    }

    private static async Task<
    Results<Ok<CurrentUserResponse>, 
    UnauthorizedHttpResult>> HandleAsync(
        HttpContext httpContext,
        IUserIdentityService identityService,
        CancellationToken cancellationToken = default)
    {
        var userIdClaim = httpContext.User
            .FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out int userId))
        {
            return TypedResults.Unauthorized();
        }

        var user = await identityService
            .GetByIdAsync(
                userId,
                cancellationToken);

        return TypedResults.Ok(
            new CurrentUserResponse(
                user.Id,
                user.Email,
                user.DisplayName,
                user.Role));
    }
}
