using Microsoft.EntityFrameworkCore;
using FluentValidation;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Domain.Suppliers;
using ProcHub.Application.Features.Users;

namespace ProcHub.Application.Features.Suppliers.Create;

public sealed class CreateSupplierHandler(
    IProcHubDbContext dbContext,
    IUserIdentityService identityService,
    IValidator<CreateSupplierCommand> validator)
{
    public async Task<SupplierResult> HandleAsync(
        CreateSupplierCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Code);

        var code = command.Code.Trim();

        await validator.ValidateAndThrowAsync(
            command, 
            cancellationToken: cancellationToken);
        
        var codeExists = await dbContext.Suppliers
            .AnyAsync(
                s => s.Code == code,
                cancellationToken);
            
        if (codeExists)
        {
            throw new DuplicateResourceException(
                $"A supplier with code {code} already exists.");
        }

        var defaultPaymentTerm = await dbContext.PaymentTerms
            .FindAsync(
                [command.DefaultPaymentTermId],
                cancellationToken)
            ?? throw new NotFoundException(
                $"PaymentTerm Id {command.DefaultPaymentTermId} not found");
        
        var mainShippingTerm = await dbContext.ShippingTerms
            .FindAsync(
                [command.MainShippingTermId],
                cancellationToken)
            ?? throw new NotFoundException(
                $"ShippingTerm Id {command.MainShippingTermId} not found.");
            
        var supplier = new Supplier(
            code,
            command.Name,
            defaultPaymentTerm,
            mainShippingTerm);
        
        supplier.SetFullName(command.FullName);
        
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
                    $"Secondary PaymentTerm Id {command.SecondaryPaymentTermId.Value} not found");
            
            supplier.SetSecondaryPaymentTerm(secondaryPaymentTerm);
        }

        if (command.SamplePaymentTermId is not null)
        {
            var samplePaymentTerm = await dbContext.PaymentTerms
                .FindAsync(
                    [command.SamplePaymentTermId.Value],
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"Sample PaymentTerm Id {command.SamplePaymentTermId.Value} not found.");
            
            supplier.SetSamplePaymentTerm(samplePaymentTerm);
        }

        supplier.SetShippingTimeDays(command.ShippingTimeDays);
        supplier.SetProductionTimeDays(command.ProductionTimeDays);
        supplier.SetMainLeadTimeDays(command.MainLeadTimeDays);
        supplier.SetSecondaryLeadTimeDays(command.SecondaryLeadTimeDays);
        supplier.SetSampleLeadTimeDays(command.SampleLeadTimeDays);

        string? buyerName = null;

        if (command.ResponsibleBuyerId is int userId)
        {
            var buyer = await identityService
                .GetByIdAsync(
                    userId,
                    cancellationToken);

            buyerName = buyer.DisplayName;
        }

        supplier.SetResponsibleBuyer(command.ResponsibleBuyerId);

        dbContext.Suppliers.Add(supplier);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new SupplierResult(
            supplier.Code,
            supplier.Name,
            supplier.DefaultPaymentTermId,
            supplier.DefaultPaymentTerm.Description,
            supplier.FullName,
            supplier.MainShippingTermId,
            supplier.MainShippingTerm.Name,
            supplier.SecondaryShippingTermId,
            supplier.SecondaryShippingTerm?.Name,
            supplier.SampleShippingTermId,
            supplier.SampleShippingTerm?.Name,
            supplier.SecondaryPaymentTermId,
            supplier.SecondaryPaymentTerm?.Description,
            supplier.SamplePaymentTermId,
            supplier.SamplePaymentTerm?.Description,
            supplier.ShippingTimeDays,
            supplier.ProductionTimeDays,
            supplier.MainLeadTimeDays,
            supplier.SecondaryLeadTimeDays,
            supplier.SampleLeadTimeDays,
            supplier.ResponsibleBuyerId,
            buyerName,
            supplier.Status,
            supplier.CreationDate);
    }
}
    

