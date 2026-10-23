using System.ComponentModel;
using ProcHub.Domain.Exceptions;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.Suppliers;

namespace ProcHub.Domain.PurchaseOrders;

public class PurchaseOrder
{
    public int Id { get; private set; }
    public string Code { get; private set; } = null!;
    public DateOnly OrderDate { get; private set; } = 
        DateOnly.FromDateTime(DateTime.Today);
    
    public PurchaseOrderType OrderType { get; private set; } 
        = PurchaseOrderType.Regular;

    public PurchaseOrderStatus OrderStatus { get; private set; }
        = PurchaseOrderStatus.Drafted;
    
    public int SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = null!;
    public string SupplierCode { get; private set; } = null!;
    public string SupplierName { get; private set; } = null!;
    public int PaymentTermId { get; private set; }
    public PaymentTerm PaymentTerm { get; private set; } = null!;
    public string PaymentTermDescription { get; private set; } = null!;
    public PaymentTimings PaymentTiming { get; private set; }
    public PaymentDateReference DueDateReference { get; private set; }
    public int BalanceDueDays { get; private set; }    
    public DateOnly? BalanceDueDate { get; private set; }
    public decimal DepositPercentage { get; private set; }
    public bool RequiresDeposit => DepositPercentage > 0m;
    public DateOnly? DepositPaidDate { get; private set; }
    public bool DepositPaid { get; private set; } = false;
    public decimal OrderAmount { get; private set; } = 0m;
    public decimal DepositAmount { get; private set; }
    public decimal BalanceAmount { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
        = PaymentStatus.UnPaid;
    public DateOnly? BalancePaidDate { get; private set; }
    public bool OrderPaid { get; private set; } = false;
    public int ShippingTermId { get; private set; }
    public ShippingTerm ShippingTerm { get; private set; } = null!;
    public string ShippingTermName { get; private set; } = null!;
    public string? ConfirmationReference { get; private set; }
    public string? InvoiceReference { get; private set; }
    public string? OrderNotes { get; private set; }
    public string? ShipmentSize { get; private set; }
    public string? ShipmentReference { get; private set; }
    
    public DateOnly? OrderConfirmationDate { get; private set; }
    public DateOnly? RequestedProductionDate { get; private set; }
    public DateOnly? ProductionDoneDate { get; private set; }
    public DateOnly? InvoiceDate { get; private set; }
    public DateOnly? ShippingDate { get; private set; }
    public DateOnly? BillOfLadingDate { get; private set; }
    public DateOnly? ArrivalDate { get; private set; }
    public DateOnly? ExpectedDeliveryDate { get; private set; }
    public DateOnly? DeliveryDate { get; private set; }

    public int? ProductionTimeDays { get; private set; }
    public int? ShippingTimeDays { get; private set; }
    public int LeadTimeDays { get; private set; } = 0;

    public bool OrderConfirmationSent { get; private set; } = false;
    public bool InvoiceSent { get; private set; } = false;
    public bool OrderInspectionDone { get; private set; } = false;
    public bool CertificateOfOriginSaved { get; private set; } = false;

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

        SetTerms(supplier
            .GetTerms(orderType));

        CalculateDeliveryDate(OrderDate);
    }

    private void CalculateDeliveryDate(DateOnly orderDate)
    {
        ExpectedDeliveryDate = orderDate
            .AddDays(LeadTimeDays);
    }

    public void SetExpectedDeliveryDate(DateOnly? expectedDeliveryDate)
    {
        ExpectedDeliveryDate = expectedDeliveryDate;
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

    private void SetOrderType(PurchaseOrderType orderType)
    {
        if (!Enum.IsDefined(orderType))
        {
            throw new DomainException(
                $"{orderType} is not a valid PurchaseOrderType.");
        }

        OrderType = orderType;
    }

    private void SetSupplier(Supplier supplier)
    {
        if (supplier is null)
        {
            throw new DomainException("Supplier is required.");
        }

        Supplier = supplier;
        SupplierCode = supplier.Code;
        SupplierName = supplier.Name;
        ProductionTimeDays = supplier.ProductionTimeDays;
        ShippingTimeDays = supplier.ShippingTimeDays;
    }

    private void SetTerms(Supplier.SupplierTerms terms)
    {
        var paymentTerm = terms.PaymentTerm;

        PaymentTerm = paymentTerm;
        PaymentTermDescription = paymentTerm.Description;
        DepositPercentage = paymentTerm.DepositPercentage;
        PaymentTiming = paymentTerm.PaymentTiming;
        DueDateReference = paymentTerm.DueDateReference;
        BalanceDueDays = paymentTerm.BalanceDueDays;

        ShippingTerm = terms.ShippingTerm;
        ShippingTermName = terms.ShippingTerm.Name;
        LeadTimeDays = terms.LeadTimeDays;

        if (OrderType == PurchaseOrderType.Express)
        {
            ShipmentSize = "Express";
        }

        CalculateBalanceDueDate();
    }

    public void SetOrderAmount(decimal orderAmount)
    {
        if (orderAmount < 0)
        {
            throw new DomainException(
                "Order amount cannot be negative.");
        }

        if (orderAmount != decimal.Round(orderAmount, 2))
        {
            throw new DomainException(
                "Order amount cannot have more than 2 decimal places.");
        }

        OrderAmount = orderAmount;

        CalculateAmounts();
    }

    private void CalculateAmounts()
    {
        if (!RequiresDeposit)
        {
            DepositAmount = 0m;
            BalanceAmount = OrderAmount;
            return;
        }

        if (PaymentStatus == PaymentStatus.UnPaid)
        {
            DepositAmount = OrderAmount * DepositPercentage;
        }

        BalanceAmount = OrderAmount - DepositAmount;
    }

    public void MarkDepositPaid(DateOnly paymentDate)
    {
        if (!RequiresDeposit)
        {
            throw new DomainException(
                "Order doesn't require a deposit.");
        }

        if (PaymentStatus != PaymentStatus.UnPaid)
        {
            throw new DomainException(
                "Deposit has been paid already.");
        }

        DepositPaidDate = paymentDate;
        DepositPaid = true;
        PaymentStatus = PaymentStatus.DepositPaid;
    }

    public void MarkOrderPaid(DateOnly paymentDate)
    {
        if (RequiresDeposit && !DepositPaid)
        {
            throw new DomainException(
                "Deposit hasn't been paid yet.");
        }

        BalancePaidDate = paymentDate;
        OrderPaid = true;
        PaymentStatus = PaymentStatus.Paid;
    }

    private void CalculateBalanceDueDate()
    {
        DateOnly? referenceDate = GetDueDateReference();

        if (referenceDate is null)
        {
            BalanceDueDate = null;
            return;
        }

        BalanceDueDate = PaymentTiming switch
        {
            PaymentTimings.On => referenceDate.Value,

            PaymentTimings.Before => 
                referenceDate.Value.AddDays(-BalanceDueDays),
            
            PaymentTimings.After =>
                referenceDate.Value.AddDays(BalanceDueDays),
            
            _ => throw new DomainException(
                    "Invalid payment Timing.")
        };
    }

    private DateOnly? GetDueDateReference()
    {
        return DueDateReference switch
        {
            PaymentDateReference.OrderDate => 
                OrderDate,
            PaymentDateReference.ProductionDoneDate => 
                ProductionDoneDate,
            PaymentDateReference.ShippingDate =>
                ShippingDate,
            PaymentDateReference.InvoiceDate =>
                InvoiceDate,    
            PaymentDateReference.DeliveryDate =>
                DeliveryDate,
            PaymentDateReference.BillOfLadingDate =>
                BillOfLadingDate,

            _ => throw new DomainException(
                "Invalid payment date reference.")            
        };
    }

    // CalculateBalanceDueDate() added to dates that are in PaymentDateReference
    public void SetOrderConfirmationDate(DateOnly? confirmDate)
    {
        OrderConfirmationDate = confirmDate;
    }

    public void SetRequestedProductionDate(DateOnly? reqProdDate)
    {
        RequestedProductionDate = reqProdDate;
    }

    public void SetProductionDoneDate(DateOnly? prodDoneDate)
    {
        ProductionDoneDate = prodDoneDate;
        CalculateBalanceDueDate();
    }

     public void SetInvoiceDate(DateOnly? invoiceDate)
    {
        InvoiceDate = invoiceDate;
        CalculateBalanceDueDate();
    }

    public void SetShippingDate(DateOnly? shippingDate)
    {
        ShippingDate = shippingDate;
        CalculateBalanceDueDate();
    }

    public void SetBillOfLadingDate(DateOnly? bolDate)
    {
        BillOfLadingDate = bolDate;
        CalculateBalanceDueDate();
    }

    public void SetArrivalDate(DateOnly? arrivalDate)
    {
        ArrivalDate = arrivalDate;
    }

    public void SetDeliveryDate(DateOnly? deliveryDate)
    {
        DeliveryDate = deliveryDate;
        CalculateBalanceDueDate();
    }

    public void SetConfirmationReference(string? confirmRef)
    {
        if (confirmRef?.Length > PurchaseOrderConstants.ConfirmationRefMaxLength)
        {
            throw new DomainException(
                 "ConfirmationReference cannot exceed " +
                $"{PurchaseOrderConstants.ConfirmationRefMaxLength} characters.");
        }

        ConfirmationReference = confirmRef;
    }

    public void SetInvoiceReference(string? invoiceRef)
    {
        if (invoiceRef?.Length > PurchaseOrderConstants.InvoiceRefMaxLength)
        {
            throw new DomainException(
                 "InvoiceReference cannot exceed " +
                $"{PurchaseOrderConstants.InvoiceRefMaxLength} characters.");
        }

        InvoiceReference = invoiceRef;
    }

    public void SetOrderNotes(string? orderNotes)
    {
        if (orderNotes?.Length > PurchaseOrderConstants.OrderNotesMaxLength)
        {
            throw new DomainException(
                "Order notes cannot exceed " +
                $"{PurchaseOrderConstants.OrderNotesMaxLength} characters.");
        }

        OrderNotes = orderNotes;
    }

    public void SetShipmentSize(string? shipmentSize)
    {
        if (shipmentSize?.Length > PurchaseOrderConstants.ShipmentSizeMaxLength)
        {
            throw new DomainException(
                 "Shipment Size cannot exceed " +
                $"{PurchaseOrderConstants.ShipmentSizeMaxLength} characters.");
        }

        ShipmentSize = shipmentSize;
    }

    public void SetShipmentReference(string? shipRef)
    {
        if (shipRef?.Length > PurchaseOrderConstants.ShipmentRefMaxLength)
        {
            throw new DomainException(
                "Shipment reference cannot exceed " +
                $"{PurchaseOrderConstants.ShipmentRefMaxLength} characters.");
        }

        ShipmentReference = shipRef;
    }

    public void MarkConfirmationSent(bool isSent)
    {
        OrderConfirmationSent = isSent;
    }

    public void MarkInvoiceSent(bool isSent)
    {
        InvoiceSent = isSent;
    }
    

    public void MarkInspectionDone(bool isDone)
    {
        OrderInspectionDone = isDone;
    }

    public void MarkCertificateOfOriginSaved(bool isSaved)
    {
        CertificateOfOriginSaved = isSaved;
    }
}