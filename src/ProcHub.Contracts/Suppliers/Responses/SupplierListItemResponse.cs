namespace ProcHub.Contracts.Suppliers.Responses;

public sealed record SupplierListItemResponse(
    string Code,
    string Name,
    string? FullName,
    string DefaultPaymentTermDescription);
