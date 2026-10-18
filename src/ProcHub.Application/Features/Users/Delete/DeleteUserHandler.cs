using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.Users.Delete;

public sealed class DeleteUserHandler(
    IUserIdentityService identityService)
{
    public async Task HandleAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await identityService.DeleteAsync(
            id,
            cancellationToken);
    }
}