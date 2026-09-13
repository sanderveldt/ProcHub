using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.ShippingTerms.Delete;

public sealed class DeleteShippingTermHandler(IProcHubDbContext dbContext)
{
    public async Task HandleAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var shippingTerm = await dbContext.ShippingTerms
            .FirstOrDefaultAsync(
                st => st.Id == id,
                cancellationToken)
            ?? throw new NotFoundException(
                $"Shipping Term with id {id} not found.");
            
        var isInUse = await dbContext.Suppliers
            .AnyAsync(supplier =>
                supplier.MainShippingTermId == id ||
                supplier.SecondaryShippingTermId == id ||
                supplier.SampleShippingTermId == id,
                cancellationToken);
        
        if (isInUse)
        {
            throw new ResourceInUseException(
                $"Shipping Term '{shippingTerm.Name} is still assigned to a Supplier.");
        }

        dbContext.ShippingTerms.Remove(shippingTerm);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}