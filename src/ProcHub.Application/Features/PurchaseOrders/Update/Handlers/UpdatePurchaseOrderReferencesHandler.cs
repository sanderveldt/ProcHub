using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.PurchaseOrders.Update;

public sealed class UpdatePurchaseOrderReferencesHandler(
    IProcHubDbContext dbContext,
    IValidator<UpdatePurchaseOrderReferencesCommand> validator)
{
    public async Task HandleAsync(
        UpdatePurchaseOrderReferencesCommand command,
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

        purchaseOrder.SetConfirmationReference(command.ConfirmationReference);
        purchaseOrder.SetInvoiceReference(command.InvoiceReference);
        purchaseOrder.SetOrderNotes(command.OrderNotes);
        purchaseOrder.SetShipmentSize(command.ShipmentSize);
        purchaseOrder.SetShipmentReference(command.ShipmentReference);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}