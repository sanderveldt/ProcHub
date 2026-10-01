namespace ProcHub.Application.Features.PurchaseOrders.Update;

public sealed record UpdatePurchaseOrderReferencesCommand(
    string Code)
{
    public string? ConfirmationReference { get; init; }
    public string? InvoiceReference { get; init; }
    public string? OrderNotes { get; init; }
    public string? ShipmentSize { get; init; }
    public string? ShipmentReference { get; init; }
    
}