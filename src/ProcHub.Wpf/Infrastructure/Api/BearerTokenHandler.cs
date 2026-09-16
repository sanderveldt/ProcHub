using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Authentication;

namespace ProcHub.Wpf.Infrastructure.Api;

public sealed class BearerTokenHandler(
    TokenStore tokenStore,
    AuthApiClient authApiClient)
    : DelegatingHandler
{
    private static readonly TimeSpan RefreshBeforeExpiration =
        TimeSpan.FromMinutes(1);
    
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage requestMessage,
        CancellationToken cancellationToken)
    {
        await RefreshTokenIfNeededAsync(cancellationToken);

        var accessToken = tokenStore.AccessToken;

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    accessToken);
        }

        return await base.SendAsync(
            requestMessage,
            cancellationToken);
    }

    private async Task RefreshTokenIfNeededAsync(
        CancellationToken cancellationToken)
    {
        if (!tokenStore.ShouldRefresh(
            RefreshBeforeExpiration))
        {
            return;
        }

        await tokenStore.RefreshLock.WaitAsync(
            cancellationToken);
        
        try
        {
            if (!tokenStore.ShouldRefresh(
                RefreshBeforeExpiration))
            {
                return;
            }

            var refreshToken = tokenStore.RefreshToken;

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return;
            }

            try
            {
                var tokenResponse = await authApiClient
                    .RefreshAsync(
                        refreshToken,
                        cancellationToken);
                
                tokenStore.SetTokens(tokenResponse);
            }
            catch (HttpRequestException exception)
                when (exception.StatusCode ==
                        HttpStatusCode.Unauthorized)
            {
                tokenStore.Clear();

                throw;
            }
        }
        finally
        {
            tokenStore.RefreshLock.Release();
        }
    }
}