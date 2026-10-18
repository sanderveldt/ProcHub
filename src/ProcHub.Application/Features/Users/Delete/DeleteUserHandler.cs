using FluentValidation;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.Users.Delete;

public sealed class DeleteUserHandler(
    IUserIdentityService identityService)
{
    public async Task HandleAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ValidationException(
                "UserId cannot be 0 or below.");
        }

        await identityService.DeleteAsync(
            id,
            cancellationToken);
    }
}