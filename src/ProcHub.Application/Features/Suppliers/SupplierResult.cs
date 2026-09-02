namespace ProcHub.Application.Features.Suppliers;

public sealed record SupplierResult(
    int Id,
    string Code,
    string Name,
    string? FullName,
    int DefaultPaymentTermId,
    int? MainShippingTermId,
    int? SecondaryShippingTermId,
    int? SampleShippingTermId,
    int? ShippingTimeDays,
    int? ProductionTimeDays,
    int? SecondaryPaymentTermId,
    int? MainLeadTimeDays,
    int? SecondaryLeadTimeDays,
    int? SampleLeadTimeDays);
