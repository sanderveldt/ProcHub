using FluentValidation;
using Microsoft.Extensions.Logging;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Users.Create;

public sealed class CreateUserHandler(
    IUserIdentityService identityService,
    IValidator<CreateUserCommand> validator,
    ILogger<CreateUserHandler> logger)
{
    public async Task<UserResult> HandleAsync(
        CreateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken);
        
        var email = command.Email.Trim();
        
        if (await identityService.EmailExistsAsync(
            email,
            cancellationToken))
        {
            throw new DuplicateResourceException(
                $"A user with email '{email}' already exists.");
        }

        var newUser = await identityService.CreateAsync(
            email,
            command.Password,
            command.Role,
            command.DisplayName?.Trim(),
            cancellationToken);
        
        logger.LogInformation(
            "User {UserId} with role {Role} created.",
            newUser.Id,
            newUser.Role);
        
        return newUser;
    }
}


