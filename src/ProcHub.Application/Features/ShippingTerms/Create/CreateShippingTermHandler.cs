using System.Data;
using FluentValidation;
using ProcHub.Application.Abstractions;
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
        
        var shippingTerm = new ShippingTerm(
            command.Name,
            command.Description);
        
        dbContext.ShippingTerms.Add(shippingTerm);

        await dbContext.SaveChangesAsync(cancellationToken);

        return shippingTerm.Name;
    }
}