using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using ProcHub.Infrastructure.Identity;
using ProcHub.Contracts.Users.Responses;

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

    private static async Task<Results<
        Ok<UserResponse>, NotFound>> HandleAsync(
            int id,
            UserManager<ApplicationUser> userManager)
    {
        var user = await userManager
            .FindByIdAsync(id.ToString());
        
        if (user is null)
        {
            return TypedResults.NotFound();
        }

        var roles = await userManager
            .GetRolesAsync(user);
        
        var role = roles.FirstOrDefault();

        if (role is null)
        {
            return TypedResults.NotFound();
        }

        var response = new UserResponse(
            user.Id,
            user.Email ?? string.Empty,
            user.DisplayName,
            role);
        
        return TypedResults.Ok(response);
    }
}

