using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.ShippingTerms.Get;

public sealed class ListShippingTermsHandler(IProcHubDbContext dbContext)
{
    public async Task<IReadOnlyList<ShippingTermListItem>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ShippingTerms
            .AsNoTracking()
            .OrderBy(st => st.Name)
            .Select(st => new ShippingTermListItem(
                st.Id,
                st.Name,
                st.Description))
            .ToListAsync(cancellationToken);
    }
}