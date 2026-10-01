using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.PurchaseOrders.Get;
using ProcHub.Contracts.PurchaseOrders.Enums;
using ProcHub.Contracts.PurchaseOrders.Responses;

namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class DashboardListEndpoint
{
    public static RouteGroupBuilder MapDashboardListEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/dashboard", HandleAsync)
             .WithName("PurchaseOrderDashboard");

        return group;
    }

    private static async Task<Ok<List<PurchaseOrderResponse>>> HandleAsync(
        DashboardListHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler
            .HandleAsync(cancellationToken);
        
        var response = result
            .Select(po => new PurchaseOrderResponse
            {
                Code = po.Code,
                OrderDate = po.OrderDate,

                OrderType = (PurchaseOrderType)po.OrderType,
                OrderStatus = (PurchaseOrderStatus)po.OrderStatus,

                SupplierId = po.SupplierId,
                SupplierCode = po.SupplierCode,
                SupplierName = po.SupplierName,

                PaymentTermId = po.PaymentTermId,
                PaymentTermDescription = po.PaymentTermDescription,

                BalanceDueDays = po.BalanceDueDays,
                BalanceDueDate = po.BalanceDueDate,

                DepositPercentage = po.DepositPercentage,
                DepositPaidDate = po.DepositPaidDate,

                OrderAmount = po.OrderAmount,   
                DepositAmount = po.DepositAmount,
                BalanceAmount = po.BalanceAmount,
                BalancePaidDate = po.BalancePaidDate,
                ShippingTermId = po.ShippingTermId,
                ShippingTermName = po.ShippingTermName,

                ConfirmationReference = po.ConfirmationReference,
                InvoiceReference = po.InvoiceReference,
                OrderNotes = po.OrderNotes,
                ShipmentSize = po.ShipmentSize,
                ShipmentReference = po.ShipmentReference,

                ConfirmationDate = po.ConfirmationDate,
                RequestedProductionDate = po.RequestedProductionDate,
                ProductionDoneDate = po.ProductionDoneDate,
                InvoiceDate = po.InvoiceDate,
                ShippingDate = po.ShippingDate,
                BillOfLadingDate = po.BillOfLadingDate,
                ArrivalDate = po.ArrivalDate,
                ExpectedDeliveryDate = po.ExpectedDeliveryDate,
                DeliveryDate = po.DeliveryDate,

                ProductionTimeDays = po.ProductionTimeDays,
                ShippingTimeDays = po.ShippingTimeDays,
                LeadTimeDays = po.LeadTimeDays,

                ConfirmationSent = po.ConfirmationSent,
                InvoiceSent = po.InvoiceSent,
                InspectionDone = po.InspectionDone,
                CertificateOfOriginSaved = po.CertificateOfOriginSaved
            })
            .ToList();

            return TypedResults.Ok(response);
    }
}
