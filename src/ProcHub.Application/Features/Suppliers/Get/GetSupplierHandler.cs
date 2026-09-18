using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Exceptions;
using ProcHub.Application.Abstractions;
using System.Data;

namespace ProcHub.Application.Features.Suppliers.Get;

public sealed class GetSupplierHandler(IProcHubDbContext dbContext)
{
    public async Task<SupplierResult> HandleAsync(
        GetSupplierQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query.Code);

        var code = query.Code.Trim();

        var supplier = await dbContext.Suppliers
            .AsNoTracking()
            .Where(s => s.Code == code)

            .Select(s => new SupplierResult(
                s.Code,
                s.Name,
                s.DefaultPaymentTermId,
                s.DefaultPaymentTerm.Description,

                s.FullName,

                s.MainShippingTermId,
                s.MainShippingTerm == null
                    ? null
                    : s.MainShippingTerm.Name,

                s.SecondaryShippingTermId,
                s.SecondaryShippingTerm != null
                    ? s.SecondaryShippingTerm.Name
                    : null,
                s.SampleShippingTermId,
                s.SampleShippingTerm != null
                    ? s.SampleShippingTerm.Name
                    : null,

                s.SecondaryPaymentTermId,
                s.SecondaryPaymentTerm != null
                    ? s.SecondaryPaymentTerm.Description
                    : null,

                s.ShippingTimeDays,
                s.ProductionTimeDays,
                s.MainLeadTimeDays,
                s.SecondaryLeadTimeDays,
                s.SampleLeadTimeDays,
                
                s.Status,
                s.CreationDate))

            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(
                $"Supplier with code {code} not found");
            
        return supplier;
    }
}