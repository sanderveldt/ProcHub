using FluentValidation;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;

namespace ProcHub.Application.Features.PurchaseOrders.Create;

public sealed class CreatePurchaseOrderValidator
    : AbstractValidator<CreatePurchaseOrderCommand>
{
    public CreatePurchaseOrderValidator()
    {
        RuleFor(po => po.Code)
            .NotEmpty()
            .WithMessage("PurchaseOrder code is required.")
            .MaximumLength(PurchaseOrderConstants.CodeMaxLength)
            .WithMessage($"PurchaseOrder code cannot exceed " +
                $"{PurchaseOrderConstants.CodeMaxLength} characters.");
        
        RuleFor(po => po.PurchaseOrderType)
            .NotEmpty()
            .Must(value => Enum.IsDefined(
                typeof(PurchaseOrderType),
                value))
            .WithMessage("Invalid PurchaseOrderType value");
        
        RuleFor(po => po.SupplierId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("SupplierId is required.");
    }
}