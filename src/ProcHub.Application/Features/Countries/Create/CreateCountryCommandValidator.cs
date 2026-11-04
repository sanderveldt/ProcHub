using FluentValidation;
using ProcHub.Domain.Countries;

namespace ProcHub.Application.Features.Countries.Create;

public sealed class CreateCountryCommandValidator
    : AbstractValidator<CreateCountryCommand>
{
    public CreateCountryCommandValidator()
    {
        RuleFor(c => c.Code)
            .NotEmpty()
            .WithMessage("Country code is required.")
            .MaximumLength(CountryConstants.CodeMaxLength)
            .WithMessage($"Country code cannot exceed " +
                $"{CountryConstants.CodeMaxLength} characters.");
        
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Country name is required.")
            .MaximumLength(CountryConstants.NameMaxLength)
            .WithMessage($"Country name cannot exceed " +
                $"{CountryConstants.NameMaxLength} characters.");
    }
}