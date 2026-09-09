namespace ProcHub.Api.Endpoints.Suppliers.Requests;

public sealed record UpdateSupplierRequest(
    string Name,
    int DefaultPaymentTermId)
{
    public string? FullName { get; init; }

    public int? MainShippingTermId { get; init; }
    public int? SecondaryShippingTermId { get; init; }
    public int? SampleShippingTermId { get; init; }

    public int? SecondaryPaymentTermId { get; init; }

    public int? ShippingTimeDays { get; init; }
    public int? ProductionTimeDays { get; init; }
    public int? MainLeadTimeDays { get; init; }
    public int? SecondaryLeadTimeDays { get; init; }
    public int? SampleLeadTimeDays { get; init; }
}