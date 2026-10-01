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

        if (command.ConfirmationReference is not null)
        {
            purchaseOrder.SetConfirmationReference(command.ConfirmationReference);
        }

        if (command.InvoiceReference is not null)
        {
            purchaseOrder.SetInvoiceReference(command.InvoiceReference);
        }

        if (command.OrderNotes is not null)
        {
            purchaseOrder.SetOrderNotes(command.OrderNotes);
        }

        if (command.ShipmentSize is not null)
        {
            purchaseOrder.SetShipmentSize(command.ShipmentSize);
        }
        
        if (command.ShipmentReference is not null)
        {
            purchaseOrder.SetShipmentReference(command.ShipmentReference);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}