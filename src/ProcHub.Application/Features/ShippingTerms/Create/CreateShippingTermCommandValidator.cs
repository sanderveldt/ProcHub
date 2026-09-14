using FluentValidation;
using ProcHub.Domain.ShippingTerms;

namespace ProcHub.Application.Features.ShippingTerms.Create;

public sealed class CreateShippingTermValidator
    : AbstractValidator<CreateShippingTermCommand>
{
    public CreateShippingTermValidator()
    {
        RuleFor(st => st.Name)
            .NotEmpty()
            .WithMessage("ShippingTerm name is required.")
            .MaximumLength(ShippingTermConstants.NameMaxLength)
            .WithMessage($"Shipping term name cannot exceed {ShippingTermConstants.NameMaxLength} characters.");
        
        RuleFor(st => st.Description)
            .NotEmpty()
            .WithMessage("ShippingTerm description is required.")
            .MaximumLength(ShippingTermConstants.DescriptionMaxLength)
            .WithMessage($"ShippingTerm description cannot exceed {ShippingTermConstants.DescriptionMaxLength} characters.");
    }
}