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

        var supplier = await dbContext.Suppliers
            .FindAsync(
                [command.SupplierId],
                cancellationToken)
            ?? throw new NotFoundException(
                $"SupplierId {command.SupplierId} not found.");
        
        var purchaseOrder = new PurchaseOrder(
            code,
            (PurchaseOrderType)
                command.PurchaseOrderType,
            supplier);
        
        dbContext.PurchaseOrders.Add(purchaseOrder);

        return new CreatePurchaseOrderResult(
            purchaseOrder.Code,
            purchaseOrder.OrderType,
            purchaseOrder.SupplierCode,
            purchaseOrder.SupplierName,

            purchaseOrder.PaymentTermDescription,
            purchaseOrder.BalanceDueDays,

            purchaseOrder.ShippingTermName,
            purchaseOrder.ProductionTimeDays,
            purchaseOrder.ShippingTimeDays,
            purchaseOrder.LeadTimeDays,

            purchaseOrder.OrderAmount);
    }
}