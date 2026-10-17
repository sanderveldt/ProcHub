namespace ProcHub.Contracts.Users.Responses;

public sealed record UserResponse(
    int Id,
    string UserName,
    string Email,
    string? DisplayName,
    string Role);