namespace ProcHub.Contracts.Suppliers.Responses;

public sealed record SupplierResponse(
    string Code,
    string Name,

    int DefaultPaymentTermId,
    string DefaultPaymentTermDescription,

    string? FullName,

    int MainShippingTermId,
    string MainShippingTermName,

    int? SecondaryShippingTermId,
    string? SecondaryShippingTermName,

    int? SampleShippingTermId,
    string? SampleShippingTermName,

    int? SecondaryPaymentTermId,
    string? SecondaryPaymentTermDescription,

    int? SamplePaymentTermId,
    string? SamplePaymentTermDescription,

    int? ShippingTimeDays,
    int? ProductionTimeDays,
    int MainLeadTimeDays,
    int? SecondaryLeadTimeDays,
    int? SampleLeadTimeDays,

    int? ResponsibleBuyerId,
    string? BuyerName,

    string Status,
    DateOnly CreationDate);