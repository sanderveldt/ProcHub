using ProcHub.Application.Features.Users;

namespace ProcHub.Application.Abstractions;

public interface IUserIdentityService
{
    Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default);
    
    Task<UserResult> CreateAsync(
        string email,
        string password,
        string role,
        string? displayName,
        CancellationToken cancellationToken = default);
}