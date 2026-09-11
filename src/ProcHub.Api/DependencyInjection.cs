using ProcHub.Api.Exceptions;
using ProcHub.Application.Authorization;

namespace ProcHub.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(
        this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()

            .AddPolicy(
                AppPolicies.SuperUserOrAdmin,
                policy => policy.RequireRole(
                    AppRoles.SuperUser,
                    AppRoles.Admin))
            
            .AddPolicy(
                AppPolicies.AdminOnly,
                policy => policy.RequireRole(
                    AppRoles.Admin));
        
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}