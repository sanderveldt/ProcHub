using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Domain.PurchaseOrders;

namespace ProcHub.Application.Features.PurchaseOrders.Update;

public sealed class UpdatePurchaseOrderDatesHandler(
    IProcHubDbContext dbContext,
    IValidator<UpdatePurchaseOrderDatesCommand> validator)
{
    public async Task HandleAsync(
        UpdatePurchaseOrderDatesCommand command,
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

        purchaseOrder.SetOrderConfirmationDate(command.OrderConfirmationDate);
        purchaseOrder.SetRequestedProductionDate(command.RequestedProductionDate);
        purchaseOrder.SetProductionDoneDate(command.ProductionDoneDate);
        purchaseOrder.SetInvoiceDate(command.InvoiceDate);
        purchaseOrder.SetShippingDate(command.ShippingDate);
        purchaseOrder.SetBillOfLadingDate(command.BillOfLadingDate);
        purchaseOrder.SetArrivalDate(command.ArrivalDate);
        purchaseOrder.SetExpectedDeliveryDate(command.ExpectedDeliveryDate);
        purchaseOrder.SetDeliveryDate(command.DeliveryDate);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}