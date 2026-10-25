using ProcHub.Contracts.PurchaseOrders.Enums;

namespace ProcHub.Contracts.PurchaseOrders.Responses;

public sealed record PurchaseOrderBySupplierListResponse
{
    public required string Code { get; init; }
    public DateOnly OrderDate { get; init; }
    public PurchaseOrderType OrderType { get; init; }
    public PurchaseOrderStatus Status { get; init; }
    public decimal OrderAmount { get; init; }
    public required string PaymentStatus { get; init; }
    public DateOnly? ConfirmationDate { get; init; }
    public DateOnly? ProductionDoneDate { get; init; }
    public DateOnly? ShippingDate { get; init; }
    public DateOnly? ArrivalDate { get; init;  }
    public DateOnly? DeliveryDate { get; init; }
}