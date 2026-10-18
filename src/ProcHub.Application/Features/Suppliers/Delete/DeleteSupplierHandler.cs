using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Suppliers.Delete;

public sealed class DeleteSupplierHandler(
    IProcHubDbContext dbContext)
{
    public async Task HandleAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ValidationException(
                "Supplier cannot be empty.");
        }

        var supplier = await dbContext.Suppliers
            .FirstOrDefaultAsync(
                s => s.Code == code,
                cancellationToken)

            ?? throw new NotFoundException(
                $"Supplier with code '{code}' not found.");

        bool isInUse = await dbContext
            .PurchaseOrders.AnyAsync(po => 
                po.SupplierId == supplier.Id &&
                po.OrderStatus != PurchaseOrderStatus.OrderClosed &&
                po.OrderStatus != PurchaseOrderStatus.Cancelled,
                cancellationToken);
        
        if (isInUse)
        {
            throw new ResourceInUseException(
                $"This supplier still has active purchase orders.");
        }
        
        dbContext.Suppliers.Remove(supplier);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}