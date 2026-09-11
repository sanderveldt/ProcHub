namespace ProcHub.Contracts.Authentication.Responses;

public sealed record TokenResponse(
    string TokenType,
    string AccessToken,
    long ExpiresIn,
    string RefreshToken);