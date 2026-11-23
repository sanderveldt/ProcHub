namespace ProcHub.Application.Features.Suppliers.Get;

public sealed record SupplierListItem(
    int Id,
    string Code,
    string Name,
    string? FullName,
    string DefaultPaymentTermDescription);
