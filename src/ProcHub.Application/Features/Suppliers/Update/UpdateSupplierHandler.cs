using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Application.Features.Users;

namespace ProcHub.Application.Features.Suppliers.Update;

public sealed class UpdateSupplierHandler(
    IProcHubDbContext dbContext,
    IUserIdentityService identityService,
    IValidator<UpdateSupplierCommand> validator)
{
    public async Task<SupplierResult> HandleAsync(
        string code,
        UpdateSupplierCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        code = code.Trim();

        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken: cancellationToken);
        
        var supplier = await dbContext.Suppliers
            .FirstOrDefaultAsync(
                s => s.Code == code,
                cancellationToken)
            ?? throw new NotFoundException(
                $"Supplier with code {code} not found.");

        var defaultPaymentTerm = await dbContext.PaymentTerms
            .FindAsync(
                [command.DefaultPaymentTermId],
                cancellationToken)
            ?? throw new NotFoundException(
                $"PaymentTerm {command.DefaultPaymentTermId} not found");

        var mainShippingTerm = await dbContext.ShippingTerms
            .FindAsync(
                [command.MainShippingTermId],
                cancellationToken)
            ?? throw new NotFoundException(
                $"ShippingTerm {command.MainShippingTermId} not found.");


        supplier.SetMainShippingTerm(mainShippingTerm);
        supplier.SetDefaultPaymentTerm(defaultPaymentTerm);
        
        supplier.SetName(command.Name);
        supplier.SetFullName(command.FullName);

        if (command.SecondaryShippingTermId is null)
        {
            supplier.SetSecondaryShippingTerm(null);
        }
        else
        {
            var secondaryShippingTerm = await dbContext.ShippingTerms
                .FindAsync(
                    [command.SecondaryShippingTermId.Value],
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"ShippingTerm {command.SecondaryShippingTermId.Value} not found.");
                
            supplier.SetSecondaryShippingTerm(secondaryShippingTerm);
        }

        if (command.SampleShippingTermId is null)
        {
            supplier.SetSampleShippingTerm(null);
        }
        else
        {
            var sampleShippingTerm = await dbContext.ShippingTerms
                .FindAsync(
                    [command.SampleShippingTermId.Value],
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"ShippingTerm {command.SampleShippingTermId.Value} not found.");
                
            supplier.SetSampleShippingTerm(sampleShippingTerm);
        }

        if (command.SecondaryPaymentTermId is null)
        {
            supplier.SetSecondaryPaymentTerm(null);
        }
        else
        {
            var secondaryPaymentTerm = await dbContext.PaymentTerms
                .FindAsync(
                    [command.SecondaryPaymentTermId.Value],
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"Secondary PaymentTerm with Id {command.SecondaryPaymentTermId.Value} not found.");
            
            supplier.SetSecondaryPaymentTerm(secondaryPaymentTerm);
        }

        if (command.SamplePaymentTermId is null)
        {
            supplier.SetSamplePaymentTerm(null);
        }
        else
        {
            var samplePaymentTerm = await dbContext.PaymentTerms
                .FindAsync(
                    [command.SamplePaymentTermId.Value],
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"Sample PaymentTerm with Id {command.SamplePaymentTermId.Value} not found.");
            
            supplier.SetSamplePaymentTerm(samplePaymentTerm);
        }

        supplier.SetShippingTimeDays(command.ShippingTimeDays);
        supplier.SetProductionTimeDays(command.ProductionTimeDays);
        supplier.SetMainLeadTimeDays(command.MainLeadTimeDays);
        supplier.SetSecondaryLeadTimeDays(command.SecondaryLeadTimeDays);
        supplier.SetSampleLeadTimeDays(command.SampleLeadTimeDays);

        UserResult? buyer = null;

        if (command.ResponsibleBuyerId is int userId)
        {
            buyer = await identityService.GetByIdAsync(
                userId,
                cancellationToken);
        }

        supplier.SetResponsibleBuyer(command.ResponsibleBuyerId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new SupplierResult(
            supplier.Code,
            supplier.Name,

            supplier.DefaultPaymentTermId,
            defaultPaymentTerm.Description,

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
            buyer?.DisplayName,

            supplier.Status,
            supplier.CreationDate);
    }
}