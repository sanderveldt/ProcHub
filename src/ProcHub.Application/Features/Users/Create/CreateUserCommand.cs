namespace ProcHub.Application.Features.Users.Create;

public sealed record CreateUserCommand(
    string Email,
    string Password,
    string Role)
{
    public string? DisplayName { get; init; }
}