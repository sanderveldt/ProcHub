using System.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.ShippingTerms.Create;

public sealed class CreateShippingTermHandler(
    IProcHubDbContext dbContext,
    IValidator<CreateShippingTermCommand> validator)
{
    public async Task<string> HandleAsync(
        CreateShippingTermCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken: cancellationToken);

        var termNameExists = await dbContext.ShippingTerms
            .AnyAsync(
                st => st.Name == command.Name,
                cancellationToken);
        
        if (termNameExists)
        {
            throw new DuplicateResourceException(
                $"Shipping term '{command.Name}' already exists.");
        }

        var shippingTerm = new ShippingTerm(
            command.Name,
            command.Description);
        
        dbContext.ShippingTerms.Add(shippingTerm);

        await dbContext.SaveChangesAsync(cancellationToken);

        return shippingTerm.Name;
    }
}