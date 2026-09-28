using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.PurchaseOrders.Create;
using ProcHub.Contracts.PurchaseOrders.Enums;
using ProcHub.Contracts.PurchaseOrders.Requests;
using ProcHub.Contracts.PurchaseOrders.Responses;

namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class CreatePurchaseOrderEndpoint
{
    public static RouteGroupBuilder MapCreatePurchaseOrderEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync)
             .WithName("CreatePurchaseOrder");
        
        return group;
    }

    private static async Task<Created<CreatePurchaseOrderResponse>> 
        HandleAsync(
            CreatePurchaseOrderRequest request,
            CreatePurchaseOrderHandler handler,
            CancellationToken cancellationToken)
    {
        var command = new CreatePurchaseOrderCommand(
            request.Code,
            (int)request.OrderType,
            request.SupplierId);
        
        var result = await handler
            .HandleAsync(
                command,
                cancellationToken);
        
        var response = new CreatePurchaseOrderResponse(
            result.Code,
            (PurchaseOrderType)result.OrderType,
            result.SupplierCode,
            result.SupplierName,
            (PurchaseOrderStatus)result.OrderStatus,
            result.PaymentTermDescription,
            result.BalanceDueDays,
            result.ShippingTermName,
            result.ProductionTimeDays,
            result.ShippingTimeDays,
            result.LeadTimeDays,
            result.OrderAmount);
        
        return TypedResults.Created(
            $"/api/purchaseorders/{response.Code}",
            response);
    }
}