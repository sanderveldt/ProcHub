using FluentValidation;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.Users.Update;

public sealed class UpdateUserHandler(
    IUserIdentityService identityService,
    IValidator<UpdateUserCommand> validator)
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
        
        return await identityService.EditAsync(
            id,
            command.UserName,
            command.Role,
            command.DisplayName,
            cancellationToken);
    }
}