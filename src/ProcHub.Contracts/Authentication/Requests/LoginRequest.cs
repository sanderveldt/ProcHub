namespace ProcHub.Contracts.Authentication.Requests;

public sealed record LoginRequest(
    string Email,
    string Password);
