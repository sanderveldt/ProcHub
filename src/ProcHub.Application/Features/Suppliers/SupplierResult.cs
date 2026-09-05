using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.Suppliers;

public sealed record SupplierResult(
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

    SupplierStatus Status,
    DateOnly CreationDate);
