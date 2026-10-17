namespace ProcHub.Contracts.Users.Requests;

public sealed record UpdateUserRequest(
    string UserName,
    string Role)
{
    public string? DisplayName { get; init; }
}