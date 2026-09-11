namespace ProcHub.Contracts.Users.Responses;

public sealed record UserResponse(
    int Id,
    string Email,
    string? DisplayName,
    string Role);