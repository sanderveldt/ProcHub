using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.PurchaseOrders.Get;
using ProcHub.Contracts.PurchaseOrders.Enums;
using ProcHub.Contracts.PurchaseOrders.Responses;

namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class GetPurchaseOrderEndpoint
{
    public static RouteGroupBuilder MapGetPurchaseOrderEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{code}", HandleAsync)
             .WithName("GetPurchaseOrder");

        return group;
    }

    private static async Task<Ok<PurchaseOrderResponse>> HandleAsync(
        string code,
        GetPurchaseOrderHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPurchaseOrderQuery(code);

        var result = await handler.HandleAsync(
            query,
            cancellationToken);
        
        var response = new 
            PurchaseOrderResponse
            {
                Code = result.Code,
                OrderDate = result.OrderDate,

                OrderType = (PurchaseOrderType)result.OrderType,
                OrderStatus = (PurchaseOrderStatus)result.OrderStatus,

                SupplierId = result.SupplierId,
                SupplierCode = result.SupplierCode,
                SupplierName = result.SupplierName,

                PaymentTermId = result.PaymentTermId,
                PaymentTermDescription = result.PaymentTermDescription,

                BalanceDueDays = result.BalanceDueDays,
                BalanceDueDate = result.BalanceDueDate,

                DepositPercentage = result.DepositPercentage,
                DepositPaidDate = result.DepositPaidDate,
                DepositPaid = result.DepositPaid,

                OrderAmount = result.OrderAmount,   
                DepositAmount = result.DepositAmount,
                BalanceAmount = result.BalanceAmount,
                BalancePaidDate = result.BalancePaidDate,
                OrderPaid = result.OrderPaid,
                ShippingTermId = result.ShippingTermId,
                ShippingTermName = result.ShippingTermName,

                ConfirmationReference = result.ConfirmationReference,
                InvoiceReference = result.InvoiceReference,
                OrderNotes = result.OrderNotes,
                ShipmentSize = result.ShipmentSize,
                ShipmentReference = result.ShipmentReference,

                ConfirmationDate = result.ConfirmationDate,
                RequestedProductionDate = result.RequestedProductionDate,
                ProductionDoneDate = result.ProductionDoneDate,
                InvoiceDate = result.InvoiceDate,
                ShippingDate = result.ShippingDate,
                BillOfLadingDate = result.BillOfLadingDate,
                ArrivalDate = result.ArrivalDate,
                ExpectedDeliveryDate = result.ExpectedDeliveryDate,
                DeliveryDate = result.DeliveryDate,

                ProductionTimeDays = result.ProductionTimeDays,
                ShippingTimeDays = result.ShippingTimeDays,
                LeadTimeDays = result.LeadTimeDays,

                ConfirmationSent = result.ConfirmationSent,
                InvoiceSent = result.InvoiceSent,
                InspectionDone = result.InspectionDone,
                CertificateOfOriginSaved = result.CertificateOfOriginSaved
            };
        
        return TypedResults.Ok(response);
    }
}