namespace ProcHub.Application.Features.Users;

public sealed record UserResult(
    int Id,
    string Email,
    string? DisplayName,
    string Role);