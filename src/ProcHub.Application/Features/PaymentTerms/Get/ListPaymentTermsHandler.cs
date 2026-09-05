using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.PaymentTerms.Get;

public sealed class ListPaymentTermsHandler(IProcHubDbContext dbContext)
{
    public async Task<IReadOnlyList<PaymentTermListItem>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.PaymentTerms
            .AsNoTracking()
            .OrderBy(pt => pt.Description)
            .Select(pt => new PaymentTermListItem(
                pt.Id,
                pt.Description))
            .ToListAsync(cancellationToken);
    }
}