namespace ProcHub.Contracts.PurchaseOrders.Requests;

public sealed record UpdatePurchaseOrderBooleansRequest
{
    public bool? OrderConfirmationSent { get; init; }
    public bool? InvoiceSent { get; init; }
    public bool? OrderInspectionDone { get; init; }
    public bool? CertificateOfOriginSaved { get; init; }
    public uint Version { get; init; }
}