namespace ProcHub.Contracts.Suppliers.Responses;

public sealed record SupplierResponse(
    string Code,
    string Name,

    int DefaultPaymentTermId,
    string DefaultPaymentTermDescription,

    string? FullName,

    int? MainShippingTermId,
    string? MainShippingTermName,

    int? SecondaryShippingTermId,
    string? SecondaryShippingTermName,

    int? SampleShippingTermId,
    string? SampleShippingTermName,

    int? SecondaryPaymentTermId,
    string? SecondaryPaymentTermDescription,

    int? ShippingTimeDays,
    int? ProductionTimeDays,
    int? MainLeadTimeDays,
    int? SecondaryLeadTimeDays,
    int? SampleLeadTimeDays,

    string Status,
    DateOnly CreationDate);