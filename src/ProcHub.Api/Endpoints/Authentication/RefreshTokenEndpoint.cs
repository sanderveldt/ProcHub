using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProcHub.Contracts.Authentication.Requests;
using ProcHub.Infrastructure.Identity;

namespace ProcHob.Api.Endpoints.Authentication;

public static class RefreshTokenEndpoint
{
    public static RouteGroupBuilder MapRefreshTokenEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("/refresh", HandleAsync)
             .WithName("RefreshToken");
        
        return group;
    }

    private static async Task<IResult> HandleAsync(
        RefreshTokenRequest request,
        SignInManager<ApplicationUser> signInManager,
        IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
        TimeProvider timeProvider)
    {
        var options = bearerTokenOptions.Get(
            IdentityConstants.BearerScheme);
        
        var refreshTicket =
            options.RefreshTokenProtector.Unprotect(
                request.RefreshToken);

        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc)
        {
            return TypedResults.Unauthorized();
        }

        if (timeProvider.GetUtcNow() >= expiresUtc)
        {
            return TypedResults.Unauthorized();
        }

        var user = await signInManager.ValidateSecurityStampAsync(
            refreshTicket.Principal);

        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        var principal =
            await signInManager.CreateUserPrincipalAsync(user);

        return TypedResults.SignIn(
            principal,
            authenticationScheme: IdentityConstants.BearerScheme);
            
    }
}