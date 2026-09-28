using ProcHub.Contracts.PurchaseOrders.Enums;

namespace ProcHub.Contracts.PurchaseOrders.Requests;

public sealed record CreatePurchaseOrderRequest(
    string Code,
    PurchaseOrderType OrderType,
    int SupplierId);