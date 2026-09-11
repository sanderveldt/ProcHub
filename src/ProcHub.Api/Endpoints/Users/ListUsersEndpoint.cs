using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProcHub.Contracts.Users.Responses;
using ProcHub.Infrastructure.Identity;

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
        UserManager<ApplicationUser> userManager,
        CancellationToken cancellationToken)
    {
        var users = await userManager.Users
            .OrderBy(u => u.DisplayName)
            .ThenBy(u => u.Email)
            .ToListAsync(cancellationToken);
        
        var response = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);

            response.Add(new UserResponse(
                user.Id,
                user.Email ?? string.Empty,
                user.DisplayName,
                roles[0]));
        }
        
        return TypedResults.Ok(response);
    }
}