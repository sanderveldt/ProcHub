using FluentValidation;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Users.Create;

public sealed class CreateUserHandler(
    IUserIdentityService identityService,
    IValidator<CreateUserCommand> validator)
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

        return await identityService.CreateAsync(
            email,
            command.Password,
            command.Role,
            command.DisplayName?.Trim(),
            cancellationToken);
    }
}


