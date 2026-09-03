using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.Suppliers.Get;

public sealed class ListSuppliersHandler(IProcHubDbContext dbContext)
{
    public async Task<IReadOnlyList<SupplierListItem>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Suppliers
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .Select(s => new SupplierListItem(
                s.Code,
                s.Name,
                s.FullName,
                s.DefaultPaymentTerm.Description))
            .ToListAsync(cancellationToken);
    }  
}