using ProcHub.Contracts.Authentication.Responses;

namespace ProcHub.Wpf.Infrastructure.Authentication;

public sealed class TokenStore
{
    private readonly Lock _syncRoot = new();

    private string? _accessToken;
    private string? _refreshToken;
    private DateTimeOffset? _accessTokenExpiresAt;

    internal SemaphoreSlim RefreshLock { get; } = 
        new(1, 1);

    public string? AccessToken
    {
        get
        {
            lock (_syncRoot)
            {
                return _accessToken;
            }
        }
    }

    public string? RefreshToken
    {
        get
        {
            lock (_syncRoot)
            {
                return _refreshToken;
            }
        }
    }

    public void SetTokens(TokenResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        if (string.IsNullOrWhiteSpace(response.AccessToken))
        {
            throw new ArgumentException(
                "The access token cannot be empty.",
                nameof(response));
        }

        if (string.IsNullOrWhiteSpace(response.RefreshToken))
        {
            throw new ArgumentException(
                "Refresh Token cannot be empty.",
                nameof(response));
        }

        if (response.ExpiresIn <= 0)
        {
            throw new ArgumentException(
                "Access token cannot be below zero.",
                nameof(response));
        }

        lock (_syncRoot)
        {
            _accessToken = response.AccessToken;
            _refreshToken = response.RefreshToken;

            _accessTokenExpiresAt = 
                DateTimeOffset.Now.AddSeconds(
                    response.ExpiresIn);
        }
    }

    public bool ShouldRefresh(
        TimeSpan refreshBeforeExpiration)
    {
        lock (_syncRoot)
        {
            if (string.IsNullOrWhiteSpace(_refreshToken))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                return true;
            }

            if (_accessTokenExpiresAt is null)
            {
                return true;
            }

            return _accessTokenExpiresAt.Value <= 
                DateTimeOffset.Now + refreshBeforeExpiration;
        }
    }

    public void Clear()
    {
        lock (_syncRoot)
        {
            _accessToken = null;
            _refreshToken = null;
            _accessTokenExpiresAt = null;
        }
    }
}