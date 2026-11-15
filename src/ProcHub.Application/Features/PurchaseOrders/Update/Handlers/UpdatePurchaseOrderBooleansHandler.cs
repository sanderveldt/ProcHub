using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.PurchaseOrders.Update;

public sealed class UpdatePurchaseOrderBooleansHandler(
    IProcHubDbContext dbContext,
    IValidator<UpdatePurchaseOrderBooleansCommand> validator)
{
    public async Task<uint> HandleAsync(
        UpdatePurchaseOrderBooleansCommand command,
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

        if (command.OrderConfirmationSent is bool isConfSent)
        {
            purchaseOrder.MarkConfirmationSent(isConfSent);
        }

        if (command.InvoiceSent is bool isInvSent)
        {
            purchaseOrder.MarkInvoiceSent(isInvSent);
        }

        if (command.OrderInspectionDone is bool isDone)
        {
            purchaseOrder.MarkInspectionDone(isDone);
        }

        if (command.CertificateOfOriginSaved is bool isSaved)
        {
            purchaseOrder.MarkCertificateOfOriginSaved(isSaved);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return purchaseOrder.Version;        
    }
}