namespace ProcHub.Application.Features.Users;

public sealed record UserResult(
    int Id,
    string UserName,
    string Email,
    string? DisplayName,
    string Role);