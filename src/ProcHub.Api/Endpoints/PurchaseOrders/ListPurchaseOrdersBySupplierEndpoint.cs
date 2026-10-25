using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.PurchaseOrders.Get;
using ProcHub.Contracts.PurchaseOrders.Enums;
using ProcHub.Contracts.PurchaseOrders.Responses;

namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class ListPurchaseOrdersBySupplierEndpoint
{
    public static RouteGroupBuilder MapListPurchaseOrdersBySupplierEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{code}/OrdersBySupplier", HandleAsync)
             .WithName("ListPurchaseOrdersBySupplier");

        return group;
    }

    private static async Task<Ok<List<
        PurchaseOrderBySupplierListResponse>>> HandleAsync(
            string supplierCode,
            ListPurchaseOrdersBySupplierHandler handler,
            CancellationToken cancellationToken)
    {
        var code = supplierCode.Trim();

        var result = await handler
            .HandleAsync(
                code,
                cancellationToken);

        var response = result
            .Select(po => new PurchaseOrderBySupplierListResponse
            {
                Code = po.Code,
                OrderDate = po.OrderDate,
                OrderType = (PurchaseOrderType)po.OrderType,
                Status = (PurchaseOrderStatus)po.Status,

                OrderAmount = po.OrderAmount,
                PaymentStatus = po.PaymentStatus.ToString(),

                ConfirmationDate = po.ConfirmationDate,
                ProductionDoneDate = po.ProductionDoneDate,
                ShippingDate = po.ShippingDate,
                ArrivalDate = po.ArrivalDate,
                DeliveryDate = po.DeliveryDate
            })
            .ToList();

        return TypedResults.Ok(response);
    }
}