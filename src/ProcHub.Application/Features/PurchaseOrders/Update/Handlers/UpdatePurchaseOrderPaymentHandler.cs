using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.PurchaseOrders.Update;

public sealed class UpdatePurchaseOrderPaymentHandler(
    IProcHubDbContext dbContext,
    IValidator<UpdatePurchaseOrderPaymentCommand> validator)
{
    public async Task HandleAsync(
        UpdatePurchaseOrderPaymentCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(
            command, 
            cancellationToken);
        
        var purchaseOrder = await dbContext.PurchaseOrders
            .SingleOrDefaultAsync(
                po => po.Code == command.Code,
                cancellationToken)
            ?? throw new NotFoundException(
                $"PurchaseOrder with code {command.Code} not found.");

        if (command.OrderAmount is not null)
        {
            purchaseOrder.SetOrderAmount(command.OrderAmount.Value);
        }

        if (command.DepositPaid == true)
        {
            var depositPaidDate = 
                command.DepositPaidDate
                ?? DateOnly.FromDateTime(DateTime.Today);
            
            purchaseOrder.MarkDepositPaid(depositPaidDate);
        }

        if (command.OrderPaid == true)
        {
            var balancePaidDate =
                command.BalancePaidDate
                ?? DateOnly.FromDateTime(DateTime.Today);
            
            purchaseOrder.MarkOrderPaid(balancePaidDate);
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}