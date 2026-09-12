using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.PaymentTerms.Delete;

public sealed class DeletePaymentTermHandler(
    IProcHubDbContext dbContext)
{
    public async Task HandleAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var paymentTerm = await dbContext.PaymentTerms
            .FirstOrDefaultAsync(
                pt => pt.Id == id,
                cancellationToken)
            ?? throw new NotFoundException(
                $"PaymentTerm with id {id} not found.");

        var isInUse = await dbContext.Suppliers
            .AnyAsync(supplier =>
                supplier.DefaultPaymentTermId == id ||
                supplier.SecondaryPaymentTermId == id,
                cancellationToken);
            
        if (isInUse)
        {
            throw new ResourceInUseException(
                $"Payment Term '{paymentTerm.Description}' is still assigned to a Supplier.");
        }

        dbContext.PaymentTerms.Remove(paymentTerm);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}