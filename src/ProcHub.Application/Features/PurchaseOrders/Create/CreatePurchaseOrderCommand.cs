namespace ProcHub.Application.Features.PurchaseOrders.Create;

public sealed record CreatePurchaseOrderCommand(
    string Code,
    int PurchaseOrderType,
    int SupplierId);