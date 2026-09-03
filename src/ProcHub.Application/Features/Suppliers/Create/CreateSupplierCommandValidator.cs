using FluentValidation;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.Suppliers.Create;

public sealed class CreateSupplierCommandValidator
    : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Supplier code is required.")
            .MaximumLength(SupplierConstants.CodeMaxLength)
            .WithMessage($"Supplier code cannot exceed {SupplierConstants.CodeMaxLength} characters.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Supplier name is required.")
            .MaximumLength(SupplierConstants.NameMaxLength)
            .WithMessage($"Supplier name cannot exceed {SupplierConstants.NameMaxLength} characters.");
        
        RuleFor(x => x.FullName)
            .MaximumLength(SupplierConstants.FullNameMaxLength)
            .WithMessage($"Supplier full name cannot exceed {SupplierConstants.FullNameMaxLength} characters.");

        RuleFor(x => x.DefaultPaymentTermId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Default payment term is required.");
        
        RuleFor(x => x.MainShippingTermId)
            .GreaterThan(0)
            .When(x => x.MainShippingTermId.HasValue);

        RuleFor(x => x.SecondaryShippingTermId)
            .GreaterThan(0)
            .When(x => x.SecondaryShippingTermId.HasValue);

        RuleFor(x => x.SampleShippingTermId)
            .GreaterThan(0)
            .When(x => x.SampleShippingTermId.HasValue);
        
        RuleFor(x => x.ShippingTimeDays)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ShippingTimeDays.HasValue)
            .WithMessage("Shipping days can't be negative.");

        RuleFor(x => x.ProductionTimeDays)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ProductionTimeDays.HasValue)
            .WithMessage("Production days can't be negative.");
        
        RuleFor(x => x.SecondaryPaymentTermId)
            .GreaterThan(0)
            .When(x => x.SecondaryPaymentTermId.HasValue);
        
        RuleFor(x => x.MainLeadTimeDays)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MainLeadTimeDays.HasValue)
            .WithMessage("Main lead time can't be negative.");
        
        RuleFor(x => x.SecondaryLeadTimeDays)
            .GreaterThanOrEqualTo(0)
            .When(x => x.SecondaryLeadTimeDays.HasValue)
            .WithMessage("Secondary lead time can't be negative.");

        RuleFor(x => x.SampleLeadTimeDays)
            .GreaterThanOrEqualTo(0)
            .When(x => x.SampleLeadTimeDays.HasValue)
            .WithMessage("Sample lead time can't be negative.");

    }
}