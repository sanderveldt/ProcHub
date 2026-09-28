using ProcHub.Contracts.PurchaseOrders.Enums;

namespace ProcHub.Contracts.PurchaseOrders.Responses;

public sealed record PurchaseOrderListItemResponse
{
    public required string Code { get; init; }
    public DateOnly OrderDate { get; init; }
    public PurchaseOrderType OrderType { get; init; }
    public PurchaseOrderStatus OrderStatus { get; init; }
    public required string SupplierCode { get; init; }
    public required string SupplierName { get; init; }
    public required string PaymentTermDescription { get; init; }
    public DateOnly? BalanceDueDate { get; init; }
    public decimal OrderAmount { get; init; }
    public required string ShippingTermName { get; init; }
    public string? ConfirmationReference { get; init; }
    public string? InvoiceReference { get; init; }
    public DateOnly? ProductionDoneDate { get; init; }
    public DateOnly? ShippingDate { get; init; }
    public DateOnly? DeliveryDate { get; init; }
}