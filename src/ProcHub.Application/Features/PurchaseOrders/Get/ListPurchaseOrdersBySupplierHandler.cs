using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace ProcHub.Application.Features.PurchaseOrders.Get;

public sealed class ListPurchaseOrdersBySupplierHandler(
    IProcHubDbContext dbContext)
{
    public async Task<List<PurchaseOrderBySupplierListItem>> HandleAsync(
        string supplierCode,
        CancellationToken cancellationToken = default)
    { 
        if (supplierCode == null)
        {
            throw new ValidationException(
                "SupplierCode can't be empty.");
        }

        var supplier = await dbContext.PurchaseOrders
            .AsNoTracking()
            .Where(s => s.SupplierCode == supplierCode)
            .Select(s => new PurchaseOrderBySupplierListItem
            {
                Code = s.Code,
                OrderDate = s.OrderDate,
                OrderType = s.OrderType,
                Status = s.OrderStatus,

                OrderAmount = s.OrderAmount,
                PaymentStatus = s.PaymentStatus,

                ConfirmationDate = s.OrderConfirmationDate,
                ProductionDoneDate = 
                    s.ProductionDoneDate ?? 
                    s.RequestedProductionDate,

                ShippingDate = s.ShippingDate,
                ArrivalDate = s.ArrivalDate,
                DeliveryDate = 
                    s.DeliveryDate ??
                    s.ExpectedDeliveryDate
            })
            .ToListAsync(cancellationToken);
            
        return supplier;
    }
}
