using FluentValidation;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.Suppliers.ChangeStatus;

public sealed class ChangeSupplierStatusCommandValidator
    : AbstractValidator<ChangeSupplierStatusCommand>
{
    public ChangeSupplierStatusCommandValidator()
    {
        RuleFor(x => x.Status)
            .Must(status => Enum.IsDefined(
                typeof(SupplierStatus), 
                status))
            .WithMessage("Invalid Supplier status.");
    }
}