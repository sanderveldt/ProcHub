using FluentValidation;

namespace ProcHub.Application.Features.ShippingTerms.Create;

public sealed class CreateShippingTermValidator
    : AbstractValidator<CreateShippingTermCommand>
{
    public CreateShippingTermValidator()
    {
        RuleFor(st => st.Name)
            .NotEmpty()
            .MaximumLength(7);
        
        RuleFor(st => st.Description)
            .NotEmpty()
            .MaximumLength(30);
    }
}