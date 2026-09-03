using FluentValidation;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.PaymentTerms.Create;

public sealed class CreatePaymentTermHandler(
    IProcHubDbContext dbContext,
    IValidator<CreatePaymentTermCommand> validator)
{
    public async Task<string> HandleAsync(
        CreatePaymentTermCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken: cancellationToken);
        
        var paymentTerm = new PaymentTerm(
            command.Description,
            command.DepositPercentage,
            command.PaymentTiming,
            command.DueDateReference,
            command.BalanceDueDays);
        
        dbContext.PaymentTerms.Add(paymentTerm);

        await dbContext.SaveChangesAsync(cancellationToken);

        return paymentTerm.Description;


    }    
}
