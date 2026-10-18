using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Authorization;
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
        if (!AppRoles.IsValid(role))
        {
            throw new ArgumentException(
                $"Invalid role: '{role}'.");
        }

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
            user.UserName,
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
            user.UserName!,
            user.Email!,
            user.DisplayName,
            role);
    }

    public async Task<List<UserResult>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var users = await userManager.Users
            .AsNoTracking()
            .OrderBy(u => u.UserName)
            .ToListAsync(cancellationToken);
        
        var response = new List<UserResult>();

        foreach (var user in users)
        {
            // N+1 query problem, should be fine < 50 users
            var roles = await userManager
                .GetRolesAsync(user);

            if (roles.Count != 1)
            {
                throw new InvalidStateException(
                    $"User {user.Id} must have 1 role.");
            }

            var role = roles[0];

            response.Add(new UserResult(
                user.Id,
                user.UserName!,
                user.Email!,
                user.DisplayName,
                role));
        }

        return response;
    }

    public async Task<UserResult> EditAsync(
        int id,
        string userName,
        string role,
        string? displayName,
        CancellationToken cancellationToken = default)
    {   
        if (!AppRoles.IsValid(role))
        {
            throw new ArgumentException(
                $"Invalid role: '{role}'.");
        }

        var user = await userManager
            .FindByIdAsync(id.ToString());
        
        if (user is null)
        {
            throw new NotFoundException(
                $"User with id '{id}' not found.");
        }

        bool isAdmin = await userManager
            .IsInRoleAsync(
                user,
                AppRoles.Admin);

        if (isAdmin)
        {
            var admins = await userManager
                .GetUsersInRoleAsync("Admin");
            
            bool isOnlyAdmin = admins.Count == 1;

            if (isOnlyAdmin && role != AppRoles.Admin)
            {
                throw new InvalidStateException(
                    $"You cant change the role of the only admin.");
            }
        }    

        await using var transaction = 
            await dbContext.Database
                .BeginTransactionAsync(cancellationToken);
        
        userName = userName.Trim();

        if (user.UserName != userName)
        {
            EnsureSucceeded(await userManager
                .SetUserNameAsync(user, userName));
        }

        displayName = displayName?.Trim();

        if (user.DisplayName != displayName)
        {
            user.DisplayName = displayName;

            EnsureSucceeded(await userManager
                .UpdateAsync(user));
        }

        var currentRoles = await userManager
            .GetRolesAsync(user);

        var rolesToRemove = currentRoles
            .Where(currentRole => !string.Equals(
                currentRole, 
                role, 
                StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (rolesToRemove.Length > 0)
        {
            EnsureSucceeded(await userManager
                .RemoveFromRolesAsync(user, rolesToRemove));
        }

        if (!currentRoles.Contains(
            role, StringComparer.OrdinalIgnoreCase))
        {
            EnsureSucceeded(await userManager
                .AddToRoleAsync(user, role));
        }

        await transaction.CommitAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task DeleteAsync(
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

        bool isAdmin = await userManager
            .IsInRoleAsync(
                user,
                AppRoles.Admin);
        
        if (isAdmin)
        {
            var admins = await userManager
                .GetUsersInRoleAsync("Admin");
            
            bool isOnlyAdmin = admins.Count == 1;

            if (isOnlyAdmin)
            {
                throw new InvalidStateException(
                    "You can't delete the only admin.");
            }
        }

        EnsureSucceeded(await userManager
            .DeleteAsync(user));
    }

    private static void EnsureSucceeded(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join("; ",
                    result.Errors.Select(e => e.Description)));
        }
    }

}