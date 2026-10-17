using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.Users.Get;

public sealed class GetUserHandler(
    IUserIdentityService identityService)
{
    public async Task<UserResult> HandleAsync(
        GetUserQuery query,
        CancellationToken cancellationToken = default)
    {
        return await identityService
            .GetByIdAsync(
                query.Id,
                cancellationToken);
        
    }
}