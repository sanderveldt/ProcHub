using FluentValidation;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.PaymentTerms.Create;

public sealed class CreatePaymentTermCommandValidator
    : AbstractValidator<CreatePaymentTermCommand>
{
    public CreatePaymentTermCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Payment term description is required.");

        RuleFor(x => x.DepositPercentage)
            .InclusiveBetween(0m, 1m)
            .WithMessage("Deposit percentage must be between 0 and 1.");

        RuleFor(x => x.PaymentTiming)
            .Must(value => Enum.IsDefined(
                typeof(PaymentTerm.PaymentTimings),
                value))
            .WithMessage("Invalid payment timing value.");

        RuleFor(x => x.DueDateReference)
            .Must(value => Enum.IsDefined(
                typeof(PaymentTerm.PaymentDateReference),
                value))
            .WithMessage("Invalid due date reference value.");

        RuleFor(x => x.BalanceDueDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Balance due days can't be negative.");
    }
}