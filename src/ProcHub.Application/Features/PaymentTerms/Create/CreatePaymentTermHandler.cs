using FluentValidation;
using ProcHub.Application.Abstractions;
using ProcHub.Domain.PaymentTerms;

namespace ProcHub.Application.Features.PaymentTerms.Create;

public sealed class CreatePaymentTermHandler(
    IProcHubDbContext dbContext,
    IValidator<CreatePaymentTermCommand> validator)
{
    public async Task<PaymentTermResult> HandleAsync(
        CreatePaymentTermCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken: cancellationToken);
        
        var paymentTerm = new PaymentTerm(
            command.Description,
            command.DepositPercentage,
            (PaymentTimings)command.PaymentTiming,
            (PaymentDateReference)command.DueDateReference,
            command.BalanceDueDays);
        
        dbContext.PaymentTerms.Add(paymentTerm);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new PaymentTermResult(
            paymentTerm.Id,
            paymentTerm.Description,
            paymentTerm.DepositPercentage,
            paymentTerm.PaymentTiming,
            paymentTerm.DueDateReference,
            paymentTerm.BalanceDueDays);
    }    
}
