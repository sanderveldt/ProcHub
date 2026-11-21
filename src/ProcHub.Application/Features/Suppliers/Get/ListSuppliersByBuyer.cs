using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.Suppliers.Get;

public sealed class ListSuppliersByBuyerHandler(
    IProcHubDbContext dbContext)
{
    public async Task<List<SupplierListItem>> HandleAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        if (userId < 0)
        {
            throw new ValidationException(
                "UserId can't be negative.");
        }

        var suppliers = await dbContext.Suppliers
            .AsNoTracking()
            .Where(s => s.ResponsibleBuyerId == userId)
            .Select(s => new SupplierListItem(
                s.Code,
                s.Name,
                s.FullName,
                s.DefaultPaymentTerm.Description))
            .ToListAsync(cancellationToken);
        
        return suppliers;
    }
}