using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Contracts.PurchaseOrders.Requests;
using ProcHub.Application.Features.PurchaseOrders.Update;

namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class UpdatePurchaseOrderReferencesEndpoint
{
    public static RouteGroupBuilder MapUpdatePurchaseOrderReferencesEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPut("/{code}/references", HandleAsync)
             .WithName("UpdatePurchaseOrderReferences");
        
        return group;
    }
    
    private static async Task<Ok<uint>> HandleAsync(
        string code,
        UpdatePurchaseOrderReferencesRequest request,
        UpdatePurchaseOrderReferencesHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePurchaseOrderReferencesCommand(code)
        {
            ConfirmationReference = request.OrderConfirmationReference,
            InvoiceReference = request.InvoiceReference,
            OrderNotes = request.OrderNotes,
            ShipmentSize = request.ShipmentSize,
            ShipmentReference = request.ShipmentReference,
            Version = request.Version
        };

        var version = await handler.HandleAsync(
            command,
            cancellationToken);
        
        return TypedResults.Ok(version);
    }
}