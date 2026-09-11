namespace ProcHub.Contracts.Authentication.Responses;

public sealed record CurrentUserResponse(
    int Id,
    string Email,
    string? DisplayName,
    string Role);