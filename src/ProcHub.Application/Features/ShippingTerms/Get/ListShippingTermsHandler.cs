using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.ShippingTerms.Get;

public sealed class ListShippingTermsHandler(IProcHubDbContext dbContext)
{
    public async Task<List<ShippingTermResult>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ShippingTerms
            .AsNoTracking()
            .OrderBy(st => st.Name)
            .Select(st => new ShippingTermResult(
                st.Id,
                st.Name,
                st.Description))
            .ToListAsync(cancellationToken);
    }
}