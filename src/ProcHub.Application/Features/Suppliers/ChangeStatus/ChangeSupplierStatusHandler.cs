using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.Suppliers.ChangeStatus;

public sealed class ChangeSupplierStatusHandler(
    IProcHubDbContext dbContext,
    IValidator<ChangeSupplierStatusCommand> validator)
{
    public async Task<SupplierStatus> HandleAsync(
        string code,
        ChangeSupplierStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        code = code.Trim();

        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken: cancellationToken);
        
        var supplier = await dbContext.Suppliers
            .FirstOrDefaultAsync(
                s => s.Code == code,
                cancellationToken)
            ?? throw new NotFoundException(
                $"No Supplier with code '{code}' exists.");
            
        var status = (SupplierStatus)command.Status;

        switch (status)
        {
            case SupplierStatus.Active:
                supplier.Activate();
                break;
            
            case SupplierStatus.Inactive:
                supplier.Deactivate();
                break;
            
            default:
                throw new InvalidOperationException(
                    $"Supplier status '{status}' doesnt exist.");
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return supplier.Status;
    }
}