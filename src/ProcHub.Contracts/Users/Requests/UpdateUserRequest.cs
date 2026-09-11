namespace ProcHub.Contracts.Users.Requests;

public sealed record UpdateUserRequest(
    string Email,
    string Role)
{
    public string? DisplayName { get; init; }
}