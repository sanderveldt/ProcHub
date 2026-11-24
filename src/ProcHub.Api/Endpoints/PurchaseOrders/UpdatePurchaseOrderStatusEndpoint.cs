using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.PurchaseOrders.Update;
using ProcHub.Contracts.PurchaseOrders.Enums;
using ProcHub.Contracts.PurchaseOrders.Requests;

namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class UpdatePurchaseOrderStatusEndpoint
{
    public static RouteGroupBuilder MapUpdatePurchaseOrderStatusEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPatch("/{code}/status", HandleAsync)
             .WithName("UpdatePurchaseOrderStatus");
        
        return group;
    }

    private static async Task<Ok<uint>> HandleAsync(
        string code,
        UpdatePurchaseOrderStatusRequest request,
        UpdatePurchaseOrderStatusHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePurchaseOrderStatusCommand(code)
        {
            OrderStatus = (int)request.OrderStatus,
            Version = request.Version
        };

        var version = await handler
            .HandleAsync(
                command,
                cancellationToken);
        
        return TypedResults.Ok(version);
    }
}