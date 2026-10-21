using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Tests.PurchaseOrders;

public class PurchaseOrderConstructorTests()

{
    private static Supplier CreateSupplier()
    {
        return new Supplier(
            "0032",
            "TestSupplier",
            CreatePaymentTerm(),
            CreateShippingTerm());
    }

    private static PaymentTerm CreatePaymentTerm()
    {
        return new PaymentTerm(
            "Test PaymentTerm",
            0.32m,
            PaymentTimings.After,
            PaymentDateReference.InvoiceDate,
            32);
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

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Constructor_CodeIsNullOrWhiteSpace_ThrowDomainException(
        string? code)
    {
        var supplier = CreateSupplier();

        Action act = () => new PurchaseOrder(
            code!,
            PurchaseOrderType.Regular,
            supplier);
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Constructor_WithCodeAtMaxLength_SetsCode()
    {
        var supplier = CreateSupplier();
        var code = new string(
            'x',
            PurchaseOrderConstants.CodeMaxLength);
        
        var purchaseOrder = new PurchaseOrder(
            code,
            PurchaseOrderType.Regular,
            supplier);

        Assert.Equal(
            code,
            purchaseOrder.Code);
    }

    [Fact]
    public void Constructor_WithCodeOverMaxLength_ThrowDomainException()
    {
        var supplier = CreateSupplier();
        var code = new string(
            'x',
            PurchaseOrderConstants.CodeMaxLength + 1);
        
        Action act = () => new PurchaseOrder(
            code,
            PurchaseOrderType.Regular,
            supplier);
        
        Assert.Throws<DomainException>(act);
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

    [Fact]
    public void Constructor_WithNullSupplier_ThrowDomainException()
    {
        Action act = () => new PurchaseOrder(
            "0032",
            PurchaseOrderType.Regular,
            null!);
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Constructor_WithInvalidOrderType_ThrowDomainException()
    {
        var supplier = CreateSupplier();
        var invalidOrderType = (PurchaseOrderType)32;

        Action act = () => new PurchaseOrder(
            "0032",
            invalidOrderType,
            supplier);
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Constructor_WithOrderDateReference_CalculateBalanceDueDate()
    {
        int balanceDueDays = 60;

        var paymentTerm = new PaymentTerm(
            "TestTerm",
            0.32m,
            PaymentTimings.After,
            PaymentDateReference.OrderDate,
            balanceDueDays);
        
        var supplier = new Supplier(
            "0032",
            "TestSupp",
            paymentTerm,
            CreateShippingTerm());

        
        var order = new PurchaseOrder(
            "3232",
            PurchaseOrderType.Regular,
            supplier);
        

        Assert.Equal(
            order.OrderDate.AddDays(balanceDueDays),
            order.BalanceDueDate);
    }

    [Fact]
    public void Constructor_WithNoDateReference_LeaveBalanceDueDateNull()
    {
        var paymentTerm = new PaymentTerm(
        "Test PaymentTerm",
        0.32m,
        PaymentTimings.After,
        PaymentDateReference.InvoiceDate,
        32);

        var supplier = CreateSupplier();
        supplier.SetDefaultPaymentTerm(paymentTerm);

        
        var purchaseOrder = new PurchaseOrder(
        "PO-0032",
        PurchaseOrderType.Regular,
        supplier);

    
        Assert.Null(purchaseOrder.InvoiceDate);
        Assert.Null(purchaseOrder.BalanceDueDate);
    }
}