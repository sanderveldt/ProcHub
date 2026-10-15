using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Application.Features.Users;
using ProcHub.Infrastructure.Persistence;

namespace ProcHub.Infrastructure.Identity;

public sealed class UserIdentityService(
    UserManager<ApplicationUser> userManager,
    ProcHubContext dbContext)
    : IUserIdentityService
{
    public async Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager
            .FindByEmailAsync(email);

        return user is not null;
    }

    public async Task<UserResult> CreateAsync(
        string email,
        string password,
        string role,
        string? displayName,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = 
            await dbContext.Database
                    .BeginTransactionAsync(
                        cancellationToken);
        
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = displayName
        };

        var userResult = await userManager
            .CreateAsync(
                user,
                password);
        
        if (!userResult.Succeeded)
        {
            if (userResult.Errors.Any(
                error => error.Code is 
                "DuplicateEmail" or "DuplicateUserName"))
            {
                throw new DuplicateResourceException(
                    $"A user with email '{email}' already exists.");
            }

            throw new ValidationException(
                userResult.Errors.Select(
                    error => new ValidationFailure(
                        error.Code
                        .StartsWith("Password")
                            ? "Password"
                            : "User",
                        error.Description)));   
        }

        var roleResult = await userManager
            .AddToRoleAsync(
                user,
                role);
        
        if (!roleResult.Succeeded)
        {
            throw new ValidationException(
                roleResult.Errors.Select(
                    error => new ValidationFailure(
                        "Role",
                        error.Description)));
        }

        await transaction.CommitAsync(cancellationToken);
        
        return new UserResult(
            user.Id,
            user.Email,
            user.DisplayName,
            role);
    }

    public async Task<UserResult> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager
            .FindByIdAsync(id.ToString());
        
        if (user is null)
        {
            throw new NotFoundException(
                $"User with id '{id}' not found.");
        }

        var roles = await userManager
            .GetRolesAsync(user);
        
        if (roles.Count != 1)
        {
            throw new InvalidStateException(
                $"A user should have 1 role.");
        }

        var role = roles[0];

        return new UserResult(
            user.Id,
            user.Email ?? string.Empty,
            user.DisplayName,
            role);
    }

}