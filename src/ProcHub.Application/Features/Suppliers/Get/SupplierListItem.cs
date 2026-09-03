namespace ProcHub.Application.Features.Suppliers.Get;

public sealed record SupplierListItem(
    string Code,
    string Name,
    string? FullName,
    string MainPaymentTermName
);