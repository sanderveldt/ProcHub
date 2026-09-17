using ProcHub.Contracts.Authentication.Responses;
using ProcHub.Wpf.Infrastructure.Api.Clients;
using ProcHub.Wpf.Infrastructure.Authentication;

namespace ProcHub.Wpf.Infrastructure.Services;

public sealed class AuthenticationService(
    AuthApiClient authApiClient,
    TokenStore tokenStore,
    AuthSession authSession)
{
    public bool IsAuthenticated =>
        authSession.IsAuthenticated;

    public CurrentUserResponse? CurrentUser =>
        authSession.CurrentUser;

    public async Task LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var tokens = await authApiClient
            .LoginAsync(
                email,
                password,
                cancellationToken);
        
        tokenStore.SetTokens(tokens);

        try
        {
            var currentUser = await authApiClient
                .GetCurrentUserAsync(
                    cancellationToken);
            
            authSession.SetCurrentUser(currentUser);
        }
        catch
        {
            ClearAuthentication();
            throw;
        }
    }

    public void Logout()
    {
        ClearAuthentication();
    }

    public void ClearAuthentication()
    {
        tokenStore.Clear();
        authSession.Clear();
    }
    
}
