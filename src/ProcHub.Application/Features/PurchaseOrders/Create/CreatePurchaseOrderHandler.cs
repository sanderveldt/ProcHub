using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;

namespace ProcHub.Application.Features.PurchaseOrders.Create;

public sealed class CreatePurchaseOrderHandler(
    IProcHubDbContext dbContext,
    IValidator<CreatePurchaseOrderCommand> validator)
{
    public async Task<CreatePurchaseOrderResult> HandleAsync(
        CreatePurchaseOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Code);

        var code = command.Code.Trim();

        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken: cancellationToken);
        
        var codeExists = await dbContext.PurchaseOrders
            .AnyAsync(
                po => po.Code == code,
                cancellationToken);
        
        if (codeExists)
        {
            throw new DuplicateResourceException(
                $"A PurchaseOrder with code {code} already exists.");
        }

        // Includes are necessary for loading supplier terms correctly.
        var supplier = await dbContext.Suppliers
            .Include(s => s.DefaultPaymentTerm)
            .Include(s => s.SecondaryPaymentTerm)
            .Include(s => s.SamplePaymentTerm)
            .Include(s => s.MainShippingTerm)
            .Include(s => s.SecondaryShippingTerm)
            .Include(s => s.SampleShippingTerm)
            .SingleOrDefaultAsync(
                s => s.Id == command.SupplierId,
                cancellationToken)
            ?? throw new NotFoundException(
                $"SupplierId {command.SupplierId} not found.");
        
        var purchaseOrder = new PurchaseOrder(
            code,
            (PurchaseOrderType)
                command.PurchaseOrderType,
            supplier);
        
        dbContext.PurchaseOrders.Add(purchaseOrder);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreatePurchaseOrderResult(
            purchaseOrder.Code,
            purchaseOrder.OrderType,
            purchaseOrder.SupplierCode,
            purchaseOrder.SupplierName,

            purchaseOrder.OrderStatus,

            purchaseOrder.PaymentTermDescription,
            purchaseOrder.BalanceDueDays,

            purchaseOrder.ShippingTermName,
            purchaseOrder.ProductionTimeDays,
            purchaseOrder.ShippingTimeDays,
            purchaseOrder.LeadTimeDays,

            purchaseOrder.OrderAmount);
    }
}
