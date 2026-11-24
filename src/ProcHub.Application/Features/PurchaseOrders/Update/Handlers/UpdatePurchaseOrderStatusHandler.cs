using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Domain.PurchaseOrders.Enums;

namespace ProcHub.Application.Features.PurchaseOrders.Update;

public sealed class UpdatePurchaseOrderStatusHandler(
    IProcHubDbContext dbContext,
    IValidator<UpdatePurchaseOrderStatusCommand> validator)
{
    public async Task<uint> HandleAsync(
        UpdatePurchaseOrderStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken: cancellationToken);
        
        var purchaseOrder = await dbContext.PurchaseOrders
            .SingleOrDefaultAsync(
                po => po.Code == command.Code,
                cancellationToken)
            ?? throw new NotFoundException(
                $"PurchaseOrder with code {command.Code} not found.");
        
        if (purchaseOrder.Version != command.Version)
        {
            throw new ConcurrencyException(
                $"PurchaseOrder '{purchaseOrder.Code}' was changed by another user.");
        }

        purchaseOrder.SetOrderStatus(
            (PurchaseOrderStatus)command.OrderStatus);

        await dbContext.SaveChangesAsync(cancellationToken);

        return purchaseOrder.Version;
    }
}