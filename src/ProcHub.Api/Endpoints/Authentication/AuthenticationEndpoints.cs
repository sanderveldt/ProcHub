namespace ProcHub.Api.Endpoints.Authentication;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/auth")
            .WithTags("Authentication");
        
        group.MapLoginEndpoint();
        group.MapRefreshTokenEndpoint();
        group.MapCurrentUserEndpoint();

        return endpoints;
    }
}