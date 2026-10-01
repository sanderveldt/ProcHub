namespace ProcHub.Contracts.PurchaseOrders.Requests;

public sealed record UpdatePurchaseOrderReferencesRequest
{
    public string? OrderConfirmationReference { get; init; }
    public string? InvoiceReference { get; init; }
    public string? OrderNotes { get; init; }
    public string? ShipmentSize { get; init; }
    public string? ShipmentReference { get; init; }
}