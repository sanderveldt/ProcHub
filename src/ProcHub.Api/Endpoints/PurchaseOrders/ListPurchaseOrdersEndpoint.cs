using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.PurchaseOrders.Get;
using ProcHub.Contracts.PurchaseOrders.Enums;
using ProcHub.Contracts.PurchaseOrders.Responses;


namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class ListPurchaseOrdersEndpoint
{
    public static RouteGroupBuilder MapListPurchaseOrdersEndpoint(
        this RouteGroupBuilder group)
    {   
        group.MapGet("/", HandleAsync)
             .WithName("ListPurchaseOrders");

        return group;
    }

    private static async Task<Ok<List<PurchaseOrderListItemResponse>>> HandleAsync(
        ListPurchaseOrdersHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler
            .HandleAsync(cancellationToken);
        
        var response = result
            .Select(po => new PurchaseOrderListItemResponse
            {
                Code = po.Code,
                OrderDate = po.OrderDate,

                OrderType = (PurchaseOrderType)po.OrderType,
                OrderStatus = (PurchaseOrderStatus)po.OrderStatus,

                SupplierCode = po.SupplierCode,                
                SupplierName = po.SupplierName,
                PaymentTermDescription = po.PaymentTermDescription,
                BalanceDueDate = po.BalanceDueDate,
                OrderAmount = po.OrderAmount,     
                ShippingTermName = po.ShippingTermName,

                ConfirmationReference = po.ConfirmationReference,  
                InvoiceReference = po.InvoiceReference,

                ProductionDoneDate = po.ProductionDoneDate,
                ShippingDate = po.ShippingDate,
                DeliveryDate = po.DeliveryDate  
            })
            .ToList();
        
            return TypedResults.Ok(response);
    }
}