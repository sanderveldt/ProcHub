using FluentValidation;

namespace ProcHub.Application.Features.PaymentTerms.Update;

public sealed class UpdatePaymentTermCommandValidator
    : AbstractValidator<UpdatePaymentTermCommand>
{
    public UpdatePaymentTermCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Payment term description is required.");

        RuleFor(x => x.DepositPercentage)
            .InclusiveBetween(0m, 1m)
            .WithMessage("Deposit percentage must be between 0 and 1.");

        RuleFor(x => x.PaymentTiming)
            .IsInEnum()
            .WithMessage("Invalid payment timing value.");

        RuleFor(x => x.DueDateReference)
            .IsInEnum()
            .WithMessage("Invalid due date reference value.");

        RuleFor(x => x.BalanceDueDays)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Balance due days can't be negative.");
    }
}