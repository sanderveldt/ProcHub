using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.PaymentTerms.Update;

public sealed class UpdatePaymentTermHandler(
    IProcHubDbContext dbContext,
    IValidator<UpdatePaymentTermCommand> validator)
{
    public async Task<PaymentTermResult> HandleAsync(
        int id,
        UpdatePaymentTermCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken: cancellationToken);
        
        var paymentTerm = await dbContext.PaymentTerms
            .FirstOrDefaultAsync(
                pt => pt.Id == id,
                cancellationToken)
            ?? throw new NotFoundException(
                $"PaymentTerm with id {id} not found.");
            
        paymentTerm.SetDescription(command.Description);
        paymentTerm.SetDepositPercentage(command.DepositPercentage);
        paymentTerm.SetPaymentTiming(command.PaymentTiming);
        paymentTerm.SetDateReference(command.DueDateReference);
        paymentTerm.SetBalanceDueDays(command.BalanceDueDays);

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