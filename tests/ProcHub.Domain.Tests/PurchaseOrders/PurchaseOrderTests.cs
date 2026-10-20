using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.Exceptions;
using Microsoft.VisualBasic;

namespace ProcHub.Domain.Tests.PurchaseOrders;

public class PurchaseOrderTests
{
    private static PurchaseOrder CreatePurchaseOrder()
    {
        return new PurchaseOrder(
            "003200",
            PurchaseOrderType.Regular,
            CreateSupplier());
    }

    private static Supplier CreateSupplier()
    {
        return new Supplier(
            "0032",
            "TestSupplier",
            CreatePaymentTerm(),
            CreateShippingTerm());
    }

    private static Supplier CreateCompleteSupplier()
    {
        var supplier = new Supplier(
            "3200",
            "CompleteSupplier",
            CreatePaymentTerm(),
            CreateShippingTerm());

        supplier.SetMainLeadTimeDays(50);
        supplier.SetSecondaryLeadTimeDays(32);
        supplier.SetSampleLeadTimeDays(25);

        return supplier;
    }

    private static PaymentTerm CreatePaymentTerm()
    {
        return new PaymentTerm(
            "Test PaymentTerm",
            0.32m,
            PaymentTimings.After,
            PaymentDateReference.OrderDate,
            60);
    }

    private static ShippingTerm CreateShippingTerm()
    {
        return new ShippingTerm(
            "FOC",
            "Free of Charge");
    }

    [Fact]
    public void Constructor_WithValidInput_InitializesPurchaseOrder()
    {
        var supplier = CreateSupplier();
        supplier.SetMainLeadTimeDays(32);
        string orderCode = "3232";

        var order = new PurchaseOrder(
            orderCode,
            PurchaseOrderType.Regular,
            supplier);


        Assert.Equal(orderCode, order.Code);
        Assert.Equal(
            PurchaseOrderType.Regular, 
            order.OrderType);
        Assert.Equal(
            PurchaseOrderStatus.Drafted, 
            order.OrderStatus);

        Assert.Same(supplier, order.Supplier);
        Assert.Equal(supplier.Code, order.SupplierCode);
        Assert.Equal(supplier.Name, order.SupplierName);
        
        Assert.Equal(0m, order.OrderAmount);
        Assert.Equal(0m, order.DepositAmount);
        Assert.Equal(0m, order.BalanceAmount);
        Assert.Equal(PaymentStatus.UnPaid, order.PaymentStatus);

        Assert.False(order.DepositPaid);
        Assert.False(order.OrderPaid);
        Assert.Null(order.DepositPaidDate);
        Assert.Null(order.BalancePaidDate);
    }

    [Fact]
    public void Constructor_WithValidSupplier_CopiesSupplierTerms()
    {
        var supplier = CreateSupplier();
        supplier.SetMainLeadTimeDays(32);

        var terms = supplier
            .GetTerms(PurchaseOrderType.Regular);
        

        var order = new PurchaseOrder(
            "PO-032",
            PurchaseOrderType.Regular,
            supplier);


        Assert.Same(terms.PaymentTerm, order.PaymentTerm);
        Assert.Equal(
            terms.PaymentTerm.Description,
            order.PaymentTermDescription);
        
        Assert.Equal(
            terms.PaymentTerm.DepositPercentage,
            order.DepositPercentage);

        Assert.Equal(
            terms.PaymentTerm.PaymentTiming,
            order.PaymentTiming);

        Assert.Equal(
            terms.PaymentTerm.DueDateReference,
            order.DueDateReference);

        Assert.Equal(
            terms.PaymentTerm.BalanceDueDays,
            order.BalanceDueDays);
        
        Assert.Same(terms.ShippingTerm, order.ShippingTerm);
        Assert.Equal(
            terms.ShippingTerm.Name,
            order.ShippingTermName);

        Assert.Equal(terms.LeadTimeDays, order.LeadTimeDays);
    }

    [Fact]
    public void Constructor_WithLeadTime_CalculatesDeliveryDate()
    {
        var supplier = CreateSupplier();
        supplier.SetMainLeadTimeDays(32);

        var order = new PurchaseOrder(
            "PO-0032",
            PurchaseOrderType.Regular,
            supplier);

        Assert.Equal(
            order.OrderDate.AddDays(32),
            order.ExpectedDeliveryDate);
    }




}