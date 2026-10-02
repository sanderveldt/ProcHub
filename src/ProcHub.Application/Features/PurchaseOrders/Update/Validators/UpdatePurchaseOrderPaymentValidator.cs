using FluentValidation;
using ProcHub.Domain.PurchaseOrders;

namespace ProcHub.Application.Features.PurchaseOrders.Update.Validators;

public sealed class UpdatePurchaseOrderPaymentValidator
    : AbstractValidator<UpdatePurchaseOrderPaymentCommand>
{
    public UpdatePurchaseOrderPaymentValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Purchase order code is required.")
            .MaximumLength(PurchaseOrderConstants.CodeMaxLength)
            .WithMessage(
                "Purchase order code cannot exceed " +
                $"{PurchaseOrderConstants.CodeMaxLength} characters.");

        RuleFor(x => x.OrderAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Purchase order amount cannot be negative.")
            .PrecisionScale(10, 2, true)
            .WithMessage("Purchase order amount cannot have more than 2 decimal places.");
    }
}