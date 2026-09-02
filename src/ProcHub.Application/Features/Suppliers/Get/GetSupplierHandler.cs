using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Exceptions;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.Suppliers.Get;

public sealed class GetSupplierHandler(
    IProcHubDbContext dbContext)
{
    public async Task<SupplierResult> HandleAsync(
        GetSupplierQuery query,
        CancellationToken cancellationToken = default)
    {
        var supplier = await dbContext.Suppliers
            .AsNoTracking()
            .Where(s => s.Code == query.Code)

            .Select(s => new SupplierResult(
                s.Id,
                s.Code,
                s.Name,
                s.FullName,
                s.DefaultPaymentTermId,
                s.MainShippingTermId,
                s.SecondaryShippingTermId,
                s.SampleShippingTermId,
                s.ShippingTimeDays,
                s.ProductionTimeDays,
                s.SecondaryPaymentTermId,
                s.MainLeadTimeDays,
                s.SecondaryLeadTimeDays,
                s.SampleLeadTimeDays))

            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(
                $"Supplier with code {query.Code} not found");
            
        return supplier;
    }
}