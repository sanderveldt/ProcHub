using ProcHub.Contracts.PurchaseOrders.Enums;

namespace ProcHub.Contracts.PurchaseOrders.Responses;

public sealed record CreatePurchaseOrderResponse(
    string Code,
    PurchaseOrderType OrderType,
    string SupplierCode,
    string SupplierName,

    PurchaseOrderStatus OrderStatus,

    string PaymentTermDescription,
    int BalanceDueDays,

    string ShippingTermName,
    int? ProductionTimeDays,
    int? ShippingTimeDays,
    int? LeadTimeDays,

    decimal OrderAmount);