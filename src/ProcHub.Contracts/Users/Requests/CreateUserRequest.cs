namespace ProcHub.Contracts.Users.Requests;

public sealed record CreateUserRequest(
    string Email,
    string Password,
    string Role)
{
    public string? DisplayName { get; init; }
}