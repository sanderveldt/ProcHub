using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Contracts.PurchaseOrders.Requests;
using ProcHub.Application.Features.PurchaseOrders.Update;

namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class UpdatePurchaseOrderDatesEndpoint
{
    public static RouteGroupBuilder MapUpdatePurchaseOrderDatesEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPut("/{code}/dates", HandleAsync)
             .WithName("UpdatePurchaseOrderDates");
        
        return group;
    }

    private static async Task<NoContent> HandleAsync(
        string code,
        UpdatePurchaseOrderDatesRequest request,
        UpdatePurchaseOrderDatesHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePurchaseOrderDatesCommand(code)
        {
            OrderConfirmationDate = request.OrderConfirmationDate,
            RequestedProductionDate = request.RequestedProductionDate,
            ProductionDoneDate = request.ProductionDoneDate,
            InvoiceDate = request.InvoiceDate,
            ShippingDate = request.ShippingDate,
            BillOfLadingDate = request.BillOfLadingDate,
            ArrivalDate = request.ArrivalDate,
            ExpectedDeliveryDate = request.ExpectedDeliveryDate,
            DeliveryDate = request.DeliveryDate
        };

        await handler.HandleAsync(
            command,
            cancellationToken);

        return TypedResults.NoContent();
    }
}