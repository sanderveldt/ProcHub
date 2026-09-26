using ProcHub.Domain.Exceptions;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Domain.PurchaseOrders;

public class PurchaseOrder
{
    public int Id { get; private set; }
    public DateOnly OrderDate { get; private set; } = 
        DateOnly.FromDateTime(DateTime.Today);
    
    public PurchaseOrderType OrderType { get; private set; } 
        = PurchaseOrderType.Regular;

    public PurchaseOrderStatus OrderStatus { get; private set; }
        = PurchaseOrderStatus.Drafted;

    public string Code { get; private set; } = null!;

    public int SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = null!;
    public string SupplierName { get; private set; } = null!;
    public int SupplierCode { get; private set; }
    public int PaymentTermId { get; private set; }
    public PaymentTerm PaymentTerm { get; private set; } = null!;
    public DateOnly? BalanceDueDate { get; private set; }
    public decimal DepositPercentage { get; private set; }
    public decimal OrderAmount { get; private set; }
    public decimal DepositAmount { get; private set; }
    public decimal BalanceAmount { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
        = PaymentStatus.UnPaid;

    public string? ConfirmationReference { get; private set; }
    public string? InvoiceReference { get; private set; }
    public string? OrderNotes { get; private set; }
    public string? ShipmentSize { get; private set; }
    public string? ShipmentReference { get; private set; }
    
    public DateOnly? ConfirmationDate { get; private set; }
    public DateOnly? RequestedReadyDate { get; private set; }
    public DateOnly? ActualReadyDate { get; private set; }
    public DateOnly? ShippingDate { get; private set; }
    public DateOnly? ArrivalDate { get; private set; }
    public DateOnly? DeliveryDate { get; private set; }

    public int? ProductionTimeDays { get; private set; }
    public int? ShippingTimeDays { get; private set; }
    public int? LeadTimeDays { get; private set; }

    public bool OrderConfirmationSent { get; private set; } = false;
    public bool InvoiceSent { get; private set; } = false;
    public bool OrderInspection { get; private set; } = false;
    public bool CertificateOfOrigin { get; private set; } = false;

    private PurchaseOrder()
    {
    }

    public PurchaseOrder(
        string code,
        PurchaseOrderType orderType,
        Supplier supplier)
    {
        SetCode(code);
        SetOrderType(orderType);
        SetSupplier(supplier);
    }

    public void SetCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new DomainException(
                "Purchase Order code is required.");
        }

        code = code.Trim();

        if (code.Length > PurchaseOrderConstants.CodeMaxLength)
        {
            throw new DomainException(
                $"PurchaseOrderCode cannot exceed {PurchaseOrderConstants.CodeMaxLength} characters.");
        }

        Code = code;
    }

    public void SetOrderType(PurchaseOrderType orderType)
    {
        if (!Enum.IsDefined(orderType))
        {
            throw new DomainException(
                $"{orderType} is not a valid PurchaseOrderType.");
        }

        OrderType = orderType;
    }

    public void SetSupplier(Supplier supplier)
    {
        Supplier = supplier;
    }


}