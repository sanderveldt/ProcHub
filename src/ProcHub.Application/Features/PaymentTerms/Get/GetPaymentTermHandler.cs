using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Application.Features.PaymentTerms;

namespace ProcHub.Application.Features.PaymentTerms.Get;

public sealed class GetPaymentTermHandler(IProcHubDbContext dbContext) 
{
    public async Task<PaymentTermResult> HandleAsync(
        GetPaymentTermQuery query,
        CancellationToken cancellationToken = default)
    {
        var paymentTerm = await dbContext.PaymentTerms
            .AsNoTracking()
            .Where(pt => pt.Id == query.Id)
            .Select(pt => new PaymentTermResult(
                pt.Id,
                pt.Description,
                pt.DepositPercentage,
                pt.PaymentTiming,
                pt.DueDateReference,
                pt.BalanceDueDays))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(
                $"Payment term with id {query.Id} not found");

        return paymentTerm;
    }
}