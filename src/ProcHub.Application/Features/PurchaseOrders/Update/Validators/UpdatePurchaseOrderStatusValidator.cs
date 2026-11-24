using FluentValidation;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;

namespace ProcHub.Application.Features.PurchaseOrders.Update.Validators;

public sealed class UpdatePurchaseOrderStatusValidator
    : AbstractValidator<UpdatePurchaseOrderStatusCommand>
{
    public UpdatePurchaseOrderStatusValidator()
    {
         RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Purchase order code is required.")
            .MaximumLength(PurchaseOrderConstants.CodeMaxLength)
            .WithMessage(
                "Purchase order code cannot exceed " +
                $"{PurchaseOrderConstants.CodeMaxLength} characters.");
        
        RuleFor(x => x.OrderStatus)
            .Must(value => Enum.IsDefined(
                typeof(PurchaseOrderStatus),
                value))
            .WithMessage("Invalid PurchaseOrder status value");  
    }
}