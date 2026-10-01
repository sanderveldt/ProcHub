namespace ProcHub.Contracts.PurchaseOrders.Requests;

public sealed record UpdatePurchaseOrderDatesRequest
{
    public DateOnly? OrderConfirmationDate { get; init; }
    public DateOnly? RequestedProductionDate { get; init; }
    public DateOnly? ProductionDoneDate { get; init; }
    public DateOnly? InvoiceDate { get; init; }
    public DateOnly? ShippingDate { get; init; }
    public DateOnly? BillOfLadingDate { get; init; }
    public DateOnly? ArrivalDate { get; init; }
    public DateOnly? ExpectedDeliveryDate { get; init; }
    public DateOnly? DeliveryDate { get; init; }
}