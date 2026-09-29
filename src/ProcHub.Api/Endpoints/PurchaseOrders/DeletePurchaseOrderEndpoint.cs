using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Authorization;
using ProcHub.Application.Features.PurchaseOrders.Delete;

namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class DeletePurchaseOrderEndpoint
{
    public static RouteGroupBuilder MapDeletePurchaseOrderEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapDelete("/{code}", HandleAsync)
             .WithName("DeletePurchaseOrder")
             .RequireAuthorization(
                AppPolicies.SuperUserOrAdmin);
        
        return group;
    }

    private static async Task<NoContent> HandleAsync(
        string code,
        DeletePurchaseOrderHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(code, cancellationToken);
        
        return TypedResults.NoContent();
    }
}