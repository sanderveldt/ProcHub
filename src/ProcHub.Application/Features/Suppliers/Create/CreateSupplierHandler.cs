using Microsoft.EntityFrameworkCore;
using FluentValidation;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.Suppliers.Create;

public sealed class CreateSupplierHandler(
    IProcHubDbContext dbContext,
    IValidator<CreateSupplierCommand> validator)
{
    public async Task<string> HandleAsync(
        CreateSupplierCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(
            command, 
            cancellationToken: cancellationToken);

        var defaultPaymentTerm = await dbContext.PaymentTerms
            .FindAsync(
                [command.DefaultPaymentTermId],
                cancellationToken)
            ?? throw new NotFoundException(
                $"PaymentTerm Id {command.DefaultPaymentTermId} not found");
            
        var supplier = new Supplier(
            command.Code,
            command.Name,
            defaultPaymentTerm);
        
        supplier.SetFullName(command.FullName);
        
        if (command.MainShippingTermId is not null)
        {
            var mainShippingTerm = await dbContext.ShippingTerms
                .FindAsync(
                    [command.MainShippingTermId.Value],
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"ShippingTerm Id {command.MainShippingTermId.Value} not found");
            
            supplier.SetMainShippingTerm(mainShippingTerm);
        }

        if (command.SecondaryShippingTermId is not null)
        {
            var secondaryShippingTerm = await dbContext.ShippingTerms
                .FindAsync(
                    [command.SecondaryShippingTermId.Value],
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"ShippingTerm Id {command.SecondaryShippingTermId.Value} not found");
            
            supplier.SetSecondaryShippingTerm(secondaryShippingTerm);
        }

        if (command.SampleShippingTermId is not null)
        {
            var sampleShippingTerm = await dbContext.ShippingTerms
                .FindAsync(
                    [command.SampleShippingTermId.Value],
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"ShippingTerm Id {command.SampleShippingTermId.Value} not found");
            
            supplier.SetSampleShippingTerm(sampleShippingTerm);
        }

        if (command.SecondaryPaymentTermId is not null)
        {
            var secondaryPaymentTerm = await dbContext.PaymentTerms
                .FindAsync(
                    [command.SecondaryPaymentTermId.Value],
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"PaymentTerm Id {command.SecondaryPaymentTermId.Value} not found");
            
            supplier.SetSecondaryPaymentTerm(secondaryPaymentTerm);
        }

        supplier.SetShippingTimeDays(command.ShippingTimeDays);
        supplier.SetProductionTimeDays(command.ProductionTimeDays);
        supplier.SetMainLeadTimeDays(command.MainLeadTimeDays);
        supplier.SetSecondaryLeadTimeDays(command.SecondaryLeadTimeDays);
        supplier.SetSampleLeadTimeDays(command.SampleLeadTimeDays);


        dbContext.Suppliers.Add(supplier);

        await dbContext.SaveChangesAsync(cancellationToken);

        return supplier.Code;
    }
}
    

