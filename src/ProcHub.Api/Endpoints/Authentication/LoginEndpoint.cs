using Microsoft.AspNetCore.Identity;
using ProcHub.Infrastructure.Identity;
using ProcHub.Contracts.Authentication;

namespace ProcHub.Api.Endpoints.Authentication;

public static class LoginEndpoint
{
    public static RouteGroupBuilder MapLoginEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("/login", HandleAsync)
             .WithName("Login");

        return group;
    }

    private static async Task<IResult> HandleAsync(
        LoginRequest request,
        SignInManager<ApplicationUser> signInManager)
    {
        signInManager.AuthenticationScheme =
            IdentityConstants.BearerScheme;
        
        var result = await signInManager.PasswordSignInAsync(
            request.Email,
            request.Password,
            isPersistent: false,
            lockoutOnFailure: true);
        
        if (!result.Succeeded)
        {
            return TypedResults.Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Login failed.",
                detail: "Invalid email or password.");
        }

        return TypedResults.Empty;
    }
}