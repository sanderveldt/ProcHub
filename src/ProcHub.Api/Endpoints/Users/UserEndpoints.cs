using ProcHub.Api.Endpoints.Users;
using ProcHub.Application.Authorization;

namespace ProcHub.Api.Endpoints.Users;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/users")
            .WithTags("Users")
            .RequireAuthorization(AppPolicies.AdminOnly);
        
        group.MapCreateUserEndpoint();
        group.MapUpdateUserEndpoint();
        group.MapGetUserEndpoint();
        group.MapListUsersEndpoint();

        return endpoints;
    }
}