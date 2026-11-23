namespace ProcHub.Contracts.Suppliers.Responses;

public sealed record SupplierListItemResponse(
    int Id,
    string Code,
    string Name,
    string? FullName,
    string DefaultPaymentTermDescription);
