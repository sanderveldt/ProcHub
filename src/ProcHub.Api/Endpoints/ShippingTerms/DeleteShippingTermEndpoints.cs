using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Authorization;
using ProcHub.Application.Features.ShippingTerms.Delete;

namespace ProcHub.Api.Endpoints.ShippingTerms;

public static class DeleteShippingTermEndpoint
{
    public static RouteGroupBuilder MapDeleteShippingTermEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", HandleAsync)
             .WithName("DeleteShippingTerm")
             .RequireAuthorization(AppPolicies.SuperUserOrAdmin);
        
        return group;
    }

    private static async Task<NoContent> HandleAsync(
        int id,
        DeleteShippingTermHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(id, cancellationToken);

        return TypedResults.NoContent();
    }
}