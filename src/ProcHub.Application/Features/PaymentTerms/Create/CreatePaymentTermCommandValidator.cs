using FluentValidation;
using ProcHub.Domain.PaymentTerms;

namespace ProcHub.Application.Features.PaymentTerms.Create;

public sealed class CreatePaymentTermCommandValidator
    : AbstractValidator<CreatePaymentTermCommand>
{
    public CreatePaymentTermCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Payment term description is required.")
            .MaximumLength(PaymentTermConstants.DescriptionMaxLength)
            .WithMessage($"PaymentTerm description cannot exceed {PaymentTermConstants.DescriptionMaxLength} characters.");

        RuleFor(x => x.DepositPercentage)
            .InclusiveBetween(0m, 1m)
            .WithMessage("Deposit percentage must be between 0 and 1.");

        RuleFor(x => x.PaymentTiming)
            .Must(value => Enum.IsDefined(
                typeof(PaymentTimings),
                value))
            .WithMessage("Invalid payment timing value.");

        RuleFor(x => x.DueDateReference)
            .Must(value => Enum.IsDefined(
                typeof(PaymentDateReference),
                value))
            .WithMessage("Invalid due date reference value.");

        RuleFor(x => x.BalanceDueDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Balance due days can't be negative.");
    }
}