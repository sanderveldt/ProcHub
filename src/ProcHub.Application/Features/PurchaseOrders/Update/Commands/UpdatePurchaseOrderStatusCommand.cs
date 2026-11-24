using ProcHub.Domain.PurchaseOrders.Enums;

namespace ProcHub.Application.Features.PurchaseOrders.Update;

public sealed record UpdatePurchaseOrderStatusCommand(
    string Code)
{
    public required int OrderStatus { get; init; }
    public uint Version { get; init; }
}