using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.ShippingTerms.Get;

public sealed class GetShippingTermHandler(IProcHubDbContext dbContext)
{
    public async Task<ShippingTermResult> HandleAsync(
        GetShippingTermQuery query,
        CancellationToken cancellationToken = default)
    {
        var shippingTerm = await dbContext.ShippingTerms
            .AsNoTracking()
            .Where(st => st.Id == query.Id)
            .Select(st => new ShippingTermResult(
                st.Id,
                st.Name,
                st.Description))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(
                $"Shipping Term with id {query.Id} not found.");
        
        return new ShippingTermResult(
            shippingTerm.Id,
            shippingTerm.Name,
            shippingTerm.Description);
    }
}