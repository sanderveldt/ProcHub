using FluentValidation;
using Microsoft.Extensions.Logging;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.Users.Update;

public sealed class UpdateUserHandler(
    IUserIdentityService identityService,
    IValidator<UpdateUserCommand> validator,
    ILogger<UpdateUserHandler> logger)
{
    public async Task<UserResult> HandleAsync(
        int id,
        UpdateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        if (int.IsNegative(id))
        {
            throw new ValidationException(
                "User id cannot be negative.");
        }

        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

        var existingUser = await identityService.GetByIdAsync(
                id,
                cancellationToken);
        
        var updatedUser = await identityService.EditAsync(
            id,
            command.UserName,
            command.Role,
            command.DisplayName,
            command.NewPassword,
            cancellationToken);

        logger.LogInformation(
            "User {UserId} updated.",
            updatedUser.Id);

        if (command.NewPassword is not null)
        {
            logger.LogInformation(
                "Password for user {UserId} reset.",
                updatedUser.Id);
        }
        
        if (existingUser.Role != updatedUser.Role)
        {
            logger.LogWarning(
                "User {UserId} role changed from {OldRule} to {NewRole}.",
                updatedUser.Id,
                existingUser.Role,
                updatedUser.Role);
        }

        return updatedUser;
    }
}