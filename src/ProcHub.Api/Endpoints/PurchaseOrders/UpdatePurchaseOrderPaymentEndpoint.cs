using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Contracts.PurchaseOrders.Requests;
using ProcHub.Application.Features.PurchaseOrders.Update;

namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class UpdatePurchaseOrderPaymentEndpoint
{
    public static RouteGroupBuilder MapUpdatePurchaseOrderPaymentEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPut("/{code}/payment", HandleAsync)
             .WithName("UpdatePurchaseOrderPayment");

        return group;
    }

    private static async Task<NoContent> HandleAsync(
        string code,
        UpdatePurchaseOrderPaymentRequest request,
        UpdatePurchaseOrderPaymentHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePurchaseOrderPaymentCommand(code)
        {
            OrderAmount = request.OrderAmount,

            DepositPaid = request.DepositPaid,
            DepositPaidDate = request.DepositPaidDate,

            OrderPaid = request.OrderPaid,
            BalancePaidDate = request.BalancePaidDate
        };

        await handler.HandleAsync(
            command,
            cancellationToken);
        
        return TypedResults.NoContent();
    }
    
}