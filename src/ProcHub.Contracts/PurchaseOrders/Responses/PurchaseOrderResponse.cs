using ProcHub.Contracts.PurchaseOrders.Enums;

namespace ProcHub.Contracts.PurchaseOrders.Responses;

public sealed record PurchaseOrderResponse
{
    public required string Code { get; init; }
    public DateOnly OrderDate { get; init; }

    public PurchaseOrderType OrderType { get; init; }
    public PurchaseOrderStatus OrderStatus { get; init; }

    public int SupplierId { get; init; }
    public required string SupplierCode { get; init; }
    public required string SupplierName { get; init; }

    public int PaymentTermId { get; init; }
    public required string PaymentTermDescription { get; init; }

    public int BalanceDueDays { get; init; }
    public DateOnly? BalanceDueDate { get; init; }

    public decimal DepositPercentage { get; init; }
    public bool DepositPaid { get; init; }
    public DateOnly? DepositPaidDate { get; init; }

    public decimal OrderAmount { get; init; }
    public decimal DepositAmount { get; init; }
    public decimal BalanceAmount { get; init; }
    public DateOnly? BalancePaidDate { get; init; }
    public bool OrderPaid { get; init; }

    public int ShippingTermId { get; init; }
    public required string ShippingTermName { get; init; }

    public string? ConfirmationReference { get; init; }
    public string? InvoiceReference { get; init; }
    public string? OrderNotes { get; init; }
    public string? ShipmentSize { get; init; }
    public string? ShipmentReference { get; init; }

    public DateOnly? ConfirmationDate { get; init; }
    public DateOnly? RequestedProductionDate { get; init; }
    public DateOnly? ProductionDoneDate { get; init; }
    public DateOnly? InvoiceDate { get; init; }
    public DateOnly? ShippingDate { get; init; }
    public DateOnly? BillOfLadingDate { get; init; }
    public DateOnly? ArrivalDate { get; init; }
    public DateOnly? ExpectedDeliveryDate { get; init; }
    public DateOnly? DeliveryDate { get; init; }

    public int? ProductionTimeDays { get; init; }
    public int? ShippingTimeDays { get; init; }
    public int? LeadTimeDays { get; init; }

    public bool ConfirmationSent { get; init; }
    public bool InvoiceSent { get; init; }
    public bool InspectionDone { get; init; }
    public bool CertificateOfOriginSaved { get; init; }
}