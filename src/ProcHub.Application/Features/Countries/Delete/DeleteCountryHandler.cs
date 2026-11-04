using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Countries.Delete;

public sealed class DeleteCountryHandler(
    IProcHubDbContext dbContext)
{
    public async Task HandleAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ValidationException(
                "Country id cannot be 0 or below.");
        }

        var country = await dbContext.Countries
            .FirstOrDefaultAsync(
                c => c.Id == id,
                cancellationToken)
            ?? throw new NotFoundException(
                $"Country with id {id} not found");
        
        bool isInUse = await dbContext.Suppliers
            .AnyAsync(supplier => 
                supplier.CountryId == id, 
                cancellationToken);
        
        if (isInUse)
        {
            throw new ResourceInUseException(
                $"Country '{country.Name}' is still assigned to a Supplier.");
        }

        dbContext.Countries.Remove(country);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}