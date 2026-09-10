namespace ProcHub.Contracts.Suppliers.Responses;

public sealed record SupplierResponse(
    string Code,
    string Name,
    string DefaultPaymentTermDescription,

    string? FullName,

    string? MainShippingTermName,
    string? SecondaryShippingTermName,
    string? SampleShippingTermName,

    string? SecondaryPaymentTermDescription,

    int? ShippingTimeDays,
    int? ProductionTimeDays,
    int? MainLeadTimeDays,
    int? SecondaryLeadTimeDays,
    int? SampleLeadTimeDays,

    string Status,
    DateOnly CreationDate);