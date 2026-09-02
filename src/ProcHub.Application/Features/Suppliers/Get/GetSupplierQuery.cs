namespace ProcHub.Application.Features.Suppliers.Get;

public sealed record GetSupplierQuery(string Code)
{
    public string? Name { get; init; }
    public string? FullName { get; init; }
    public int? DefaultPaymentTermId { get; init; }
    public int? MainShippingTermId { get; init; }
    public int? SecondaryShippingTermId { get; init; }
    public int? SampleShippingTermId { get; init; }
    public int? ShippingTimeDays { get; init; }
    public int? ProductionTimeDays { get; init; }
    public int? SecondaryPaymentTermId { get; init; }
    public int? MainLeadTimeDays { get; init; }
    public int? SecondaryLeadTimeDays { get; init; }
    public int? SampleLeadTimeDays { get; init; }
}
