using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.PurchaseOrders.Get;

public sealed class DashboardListHandler(IProcHubDbContext dbContext)
{
    public async Task<List<PurchaseOrderResult>> HandleAsync(
        CancellationToken cancellationToken)
    {
        return await dbContext.PurchaseOrders
            .AsNoTracking()
            .OrderBy(po => po.OrderDate)

            .Select(po => new PurchaseOrderResult
            {
                Code = po.Code,
                OrderDate = po.OrderDate,

                OrderType = po.OrderType,
                OrderStatus = po.OrderStatus,

                SupplierId = po.SupplierId,
                SupplierCode = po.Supplier.Code,
                SupplierName = po.Supplier.Name,

                PaymentTermId = po.PaymentTermId,
                PaymentTermDescription = po.PaymentTerm.Description,

                BalanceDueDate = po.BalanceDueDate,

                DepositPercentage = po.DepositPercentage,
                DepositPaidDate = po.DepositPaidDate,

                OrderAmount = po.OrderAmount,   
                DepositAmount = po.DepositAmount,
                BalanceAmount = po.BalanceAmount,
                OrderPaidDate = po.OrderPaidDate,

                ShippingTermId = po.ShippingTermId,
                ShippingTermName = po.ShippingTerm.Name,

                ConfirmationReference = po.ConfirmationReference,
                InvoiceReference = po.InvoiceReference,
                OrderNotes = po.OrderNotes,
                ShipmentSize = po.ShipmentSize,
                ShipmentReference = po.ShipmentReference,

                ConfirmationDate = po.OrderConfirmationDate,
                RequestedProductionDate = po.RequestedProductionDate,
                ProductionDoneDate = po.ProductionDoneDate,
                InvoiceDate = po.InvoiceDate,
                ShippingDate = po.ShippingDate,
                BillOfLadingDate = po.BillOfLadingDate,
                ArrivalDate = po.ArrivalDate,
                DeliveryDate = po.DeliveryDate,

                ProductionTimeDays = po.ProductionTimeDays,
                ShippingTimeDays = po.ShippingTimeDays,
                LeadTimeDays = po.LeadTimeDays,

                ConfirmationSent = po.OrderConfirmationSent,
                InvoiceSent = po.InvoiceSent,
                InspectionDone = po.OrderInspectionDone,
                CertificateOfOriginSaved = po.CertificateOfOriginSaved  
            })
            .ToListAsync(cancellationToken);
    }       
}