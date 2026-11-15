using FluentValidation;
using ProcHub.Domain.PurchaseOrders;

namespace ProcHub.Application.Features.PurchaseOrders.Update.Validators;

public sealed class UpdatePurchaseOrderBooleansValidator
    : AbstractValidator<UpdatePurchaseOrderBooleansCommand>
{
    public UpdatePurchaseOrderBooleansValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Purchase order code is required.")
            .MaximumLength(PurchaseOrderConstants.CodeMaxLength)
            .WithMessage(
                "Purchase order code cannot exceed " +
                $"{PurchaseOrderConstants.CodeMaxLength} characters."); 
    }
}