using FluentValidation;
using ProcHub.Domain.PurchaseOrders;

namespace ProcHub.Application.Features.PurchaseOrders.Update.Validators;

public sealed class UpdatePurchaseOrderReferencesValidator
    : AbstractValidator<UpdatePurchaseOrderReferencesCommand>
{
    public UpdatePurchaseOrderReferencesValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Purchase order code is required.")
            .MaximumLength(PurchaseOrderConstants.CodeMaxLength)
            .WithMessage(
                "Purchase order code cannot exceed " +
                $"{PurchaseOrderConstants.CodeMaxLength} characters.");
        
        RuleFor(x => x.ConfirmationReference)
            .MaximumLength(PurchaseOrderConstants.ConfirmationRefMaxLength)
            .WithMessage(
                "Confirmation reference cannot exceed " +
                $"{PurchaseOrderConstants.ConfirmationRefMaxLength} characters.");
        
        RuleFor(x => x.InvoiceReference)
            .MaximumLength(PurchaseOrderConstants.InvoiceRefMaxLength)
            .WithMessage(
                "Invoice reference cannot exceed " +
                $"{PurchaseOrderConstants.InvoiceRefMaxLength} characters.");
        
        RuleFor(x => x.OrderNotes)
            .MaximumLength(PurchaseOrderConstants.OrderNotesMaxLength)
            .WithMessage(
                "Order notes cannot exceed " +
                $"{PurchaseOrderConstants.OrderNotesMaxLength} characters.");
        
        RuleFor(x => x.ShipmentSize)
            .MaximumLength(PurchaseOrderConstants.ShipmentSizeMaxLength)
            .WithMessage(
                "Shipment size cannot exceed " +
                $"{PurchaseOrderConstants.ShipmentSizeMaxLength} characters.");
        
        RuleFor(x => x.ShipmentReference)
            .MaximumLength(PurchaseOrderConstants.ShipmentRefMaxLength)
            .WithMessage(
                "Shipment reference cannot exceed " +
                $"{PurchaseOrderConstants.ShipmentRefMaxLength} characters.");
    }
}