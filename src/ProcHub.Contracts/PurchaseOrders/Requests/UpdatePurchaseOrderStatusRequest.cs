using ProcHub.Contracts.PurchaseOrders.Enums;

namespace ProcHub.Contracts.PurchaseOrders.Requests;

public sealed record UpdatePurchaseOrderStatusRequest()
{
    public required PurchaseOrderStatus OrderStatus { get; init; }
    public uint Version { get; init; }
}