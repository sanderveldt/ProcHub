using ProcHub.Domain.PurchaseOrders.Enums;

namespace ProcHub.Application.Features.PurchaseOrders.Create;

public sealed record CreatePurchaseOrderResult(
    string Code,
    PurchaseOrderType OrderType,
    string SupplierCode,
    string SupplierName,

    string PaymentTermDescription,
    int BalanceDueDays,

    string ShippingTermName,
    int? ProductionTimeDays,
    int? ShippingTimeDays,
    int? LeadTimeDays,

    decimal OrderAmount);