using Microsoft.AspNetCore.Identity;
using ProcHub.Application.Authorization;
using ProcHub.Infrastructure.Identity;
using ProcHub.Contracts.Users.Requests;
using ProcHub.Contracts.Users.Responses;

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

    private static async Task<IResult> HandleAsync(
        int id,
        UpdateUserRequest request,
        UserManager<ApplicationUser> userManager)
    {
        if (!AppRoles.IsValid(request.Role))
        {
            return TypedResults.BadRequest(
                $"Role '{request.Role}' is invalid.");
        }

        var user = await userManager.FindByIdAsync(id.ToString());
        
        if (user is null)
        {
            return TypedResults.NotFound();
        }

        var email = request.Email.Trim();

        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser is not null && existingUser.Id != user.Id)
        {
            return TypedResults.Conflict(
                $"A user with email '{email}' already exists.");
        }

        user.Email = email;
        user.UserName = email;
        user.DisplayName = request.DisplayName?.Trim();

        var updateResult = await userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            var errors = updateResult.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(e => e.Description)
                        .ToArray());
                
            return TypedResults.ValidationProblem(errors);
        }

        var currentRoles = await userManager.GetRolesAsync(user);

        if (!currentRoles.Contains(
            request.Role,
            StringComparer.OrdinalIgnoreCase))
        {
            var addRoleResult = await userManager
                .AddToRoleAsync(
                    user,
                    request.Role);
                
            if (!addRoleResult.Succeeded)
            {
                return TypedResults.BadRequest(
                    "New role could not be assigned.");
            }

            if (currentRoles.Count > 0)
            {
                var removeRoleResult = await userManager
                    .RemoveFromRolesAsync(
                        user,
                        currentRoles);
                
                if (!removeRoleResult.Succeeded)
                {
                    return TypedResults.BadRequest(
                        "The previous role couldn't be removed.");
                }       
            }
        }

        var response = new UserResponse(
            user.Id,
            user.Email ?? string.Empty,
            user.DisplayName,
            request.Role);

        return TypedResults.Ok(response);
    }
}