using Microsoft.AspNetCore.Identity;
using ProcHub.Contracts.Users.Requests;
using ProcHub.Contracts.Users.Responses;
using ProcHub.Infrastructure.Identity;
using ProcHub.Application.Authorization;

namespace ProcHub.Api.Endpoints.Users;

public static class CreateUserEndpoint
{
    public static RouteGroupBuilder MapCreateUserEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync);

        return group;
    }

    private static async Task<IResult> HandleAsync(
        CreateUserRequest request,
        UserManager<ApplicationUser> userManager)
    {
        if (!AppRoles.IsValid(request.Role))
        {
            return TypedResults.BadRequest(
                $"Role '{request.Role}' is invalid.");
        }

        var existingUser =
            await userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return TypedResults.Conflict(
                $"A user with email '{request.Email}' already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email.Trim(),
            Email = request.Email.Trim(),
            DisplayName = request.DisplayName?.Trim()
        };

        var createResult = await userManager.CreateAsync(
            user,
            request.Password);

        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors
                .ToDictionary(
                    e => e.Code,
                    e => new[] { e.Description });

            return TypedResults.ValidationProblem(errors);
        }

        var roleResult = await userManager.AddToRoleAsync(
            user,
            request.Role);

        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);

            return TypedResults.BadRequest(
                "The user was created but the role could not be assigned.");
        }

        return TypedResults.Created(
            $"/api/users/{user.Id}",
            new UserResponse(
                user.Id,
                user.Email!,
                user.DisplayName,
                request.Role));
    }
}