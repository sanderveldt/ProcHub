namespace ProcHub.Application.Features.Users.Update;

public sealed record UpdateUserCommand(
    string UserName,
    string Role)
{
    public string? DisplayName { get; init; }
}