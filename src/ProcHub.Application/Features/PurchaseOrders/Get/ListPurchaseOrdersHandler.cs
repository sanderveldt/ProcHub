using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.PurchaseOrders.Get;

public sealed class ListPurchaseOrdersHandler(IProcHubDbContext dbContext)
{
    public async Task<List<PurchaseOrderListItem>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.PurchaseOrders
            .AsNoTracking()
            .OrderBy(po => po.Code)

            .Select(po => new PurchaseOrderListItem
            {
                Code = po.Code,
                OrderDate = po.OrderDate,

                OrderType = po.OrderType,
                OrderStatus = po.OrderStatus,

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
            .ToListAsync(cancellationToken);
    }
}