using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using ProcHub.Contracts.Authentication.Responses;
using ProcHub.Infrastructure.Identity;

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
        UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.GetUserAsync(httpContext.User);

        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        var roles = await userManager.GetRolesAsync(user);

        var role = roles.FirstOrDefault();

        if (role is null)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok(new CurrentUserResponse(
            user.Id,
            user.Email ?? string.Empty,
            user.DisplayName,
            role));
    }
}
