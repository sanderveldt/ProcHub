using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.PurchaseOrders.Update;
using ProcHub.Contracts.PurchaseOrders.Requests;

namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class UpdatePurchaseOrderBooleansEndpoint
{
    public static RouteGroupBuilder MapUpdatePurchaseOrderBooleansEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPut("/{code}/booleans", HandleAsync)
             .WithName("UpdatePurchaseOrderBooleans");
        
        return group;
    }

    private static async Task<Ok<uint>> HandleAsync(
        string code,
        UpdatePurchaseOrderBooleansRequest request,
        UpdatePurchaseOrderBooleansHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePurchaseOrderBooleansCommand(code)
        {
            OrderConfirmationSent = request.OrderConfirmationSent,
            InvoiceSent = request.InvoiceSent,
            OrderInspectionDone = request.OrderInspectionDone,
            CertificateOfOriginSaved = request.CertificateOfOriginSaved,
            Version = request.Version
        };

        var version = await handler
            .HandleAsync(
                command,
                cancellationToken);

        return TypedResults.Ok(version);
    }
}