using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.Suppliers.Create;

public sealed record SupplierListItem(
    string Code,
    string Name,
    string? FullName,
    string MainPaymentTermName
);