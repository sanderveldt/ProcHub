using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.Users.Get;

public sealed class ListUsersHandler(
    IUserIdentityService identityService)
{
    public async Task<List<UserResult>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        return await identityService
            .ListAsync(cancellationToken);
    }
}