namespace ProcHub.Application.Features.PurchaseOrders.Update;

public sealed record UpdatePurchaseOrderBooleansCommand(
    string Code)
{
    public bool? OrderConfirmationSent { get; init; }
    public bool? InvoiceSent { get; init; }
    public bool? OrderInspectionDone { get; init; }
    public bool? CertificateOfOriginSaved { get; init; }
    public uint Version { get; init; }
}