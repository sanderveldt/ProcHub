using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Tests.PurchaseOrders;

public class PurchaseOrderTests
{
    private static PurchaseOrder CreatePurchaseOrder(
        Supplier? supplier = null)
    {
        return new PurchaseOrder(
            "PO-32",
            PurchaseOrderType.Regular,
            supplier ?? CreateSupplier());
    }

    private static Supplier CreateSupplier(
        PaymentTerm? paymentTerm = null)
    {
        return new Supplier(
            "0032",
            "TestSupplier",
            paymentTerm ?? CreatePaymentTerm(),
            CreateShippingTerm());
    }

    private static PaymentTerm CreatePaymentTerm(
        decimal? depositPercentage = null)
    {
        return new PaymentTerm(
            "TestTerm",
            depositPercentage ?? 0.00m,
            PaymentTimings.After,
            PaymentDateReference.OrderDate,
            32);
    }

    private static ShippingTerm CreateShippingTerm()
    {
        return new ShippingTerm(
            "TestTerm",
            "Just for Testing");
    }

    private static DateOnly CreateTestDate()
    {
        return DateOnly
            .FromDateTime(DateTime.Today);
    }


    // SetOrderAmount()

    [Fact]
    public void SetOrderAmount_WithZeroAmount_SetsOrderAmount()
    {
        var purchaseOrder = CreatePurchaseOrder();
        decimal orderAmount = 0.00m;

        purchaseOrder.SetOrderAmount(orderAmount);

        Assert.Equal(
            orderAmount, 
            purchaseOrder.OrderAmount);
    }

    [Theory]
    [InlineData(0.05)]
    [InlineData(1.05)]
    [InlineData(32.32)]
    [InlineData(9999.99)]
    public void SetOrderAmount_WithValidAmount_SetsOrderAmount(
        decimal orderAmount)
    {
        var purchaseOrder = CreatePurchaseOrder();

        purchaseOrder.SetOrderAmount(orderAmount);

        Assert.Equal(
            orderAmount,
            purchaseOrder.OrderAmount);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-0.32)]
    [InlineData(-1)]
    [InlineData(-32)]
    public void SetOrderAmount_WithNegativeAmount_ThrowDomainException(
        decimal orderAmount)
    {
        var purchaseOrder = CreatePurchaseOrder();

        Action act = () => purchaseOrder
            .SetOrderAmount(orderAmount);
        
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(0.001)]
    [InlineData(32.0032)]
    [InlineData(999.999)]
    public void SetOrderAmount_WithOverTwoDecimals_ThrowDomainException(
        decimal orderAmount)
    {
        var purchaseOrder = CreatePurchaseOrder();

        Action act = () => purchaseOrder
            .SetOrderAmount(orderAmount);

        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void SetOrderAmount_WithDeposit_CalculateDepositAndBalance()
    {
        decimal depositPercentage = 0.30m;
        decimal orderAmount = 100.00m;
        decimal expectedDepositAmount = 30.00m;
        decimal expectedBalanceAmount = 70.00m;

        var paymentTerm = CreatePaymentTerm();
        paymentTerm.SetDepositPercentage(depositPercentage);

        var supplier = new Supplier(
            "032",
            "TestSupp",
            paymentTerm,
            CreateShippingTerm());
        
        var purchaseOrder = new PurchaseOrder(
            "0302",
            PurchaseOrderType.Regular,
            supplier);
        

        purchaseOrder.SetOrderAmount(orderAmount);


        Assert.True(purchaseOrder.RequiresDeposit);
        Assert.False(purchaseOrder.DepositPaid);
        Assert.Null(purchaseOrder.DepositPaidDate);

        Assert.Equal(
            PaymentStatus.UnPaid,
            purchaseOrder.PaymentStatus);

        Assert.Equal(
            orderAmount,
            purchaseOrder.OrderAmount);

        Assert.Equal(
            expectedDepositAmount,
            purchaseOrder.DepositAmount);
        
        Assert.Equal(
            expectedBalanceAmount,
            purchaseOrder.BalanceAmount);
    }

    [Fact]
    public void SetOrderAmount_WithoutDeposit_SetFullBalance()
    {
        decimal depositPercentage = 0.0m;
        decimal orderAmount = 100.00m;
        decimal expectedDepositAmount = 00.00m;
        decimal expectedBalanceAmount = 100.00m;

        var paymentTerm = CreatePaymentTerm();
        paymentTerm.SetDepositPercentage(depositPercentage);

        var supplier = new Supplier(
            "032",
            "TestSupp",
            paymentTerm,
            CreateShippingTerm());
        
        var purchaseOrder = new PurchaseOrder(
            "0302",
            PurchaseOrderType.Regular,
            supplier);
        

        purchaseOrder.SetOrderAmount(orderAmount);

        Assert.Equal(
            orderAmount,
            purchaseOrder.OrderAmount);
        
        Assert.Equal(
            expectedDepositAmount,
            purchaseOrder.DepositAmount);
        
        Assert.Equal(
            expectedBalanceAmount,
            purchaseOrder.BalanceAmount);
    }

    [Fact]
    public void SetOrderAmount_BeforeDepositPaid_RecalculatesDepositAmount()
    {
        decimal depositPercentage = 0.50m;
        decimal orderAmount = 100.00m;
        decimal newOrderAmount = 150.00m;
        decimal expectedDepositAmount = 75.00m;
        decimal expectedBalanceAmount = 75.00m;

        var paymentTerm = CreatePaymentTerm();
        paymentTerm.SetDepositPercentage(depositPercentage);

        var supplier = new Supplier(
            "032",
            "TestSupp",
            paymentTerm,
            CreateShippingTerm());
        
        var purchaseOrder = new PurchaseOrder(
            "0320",
            PurchaseOrderType.Regular,
            supplier);
        purchaseOrder.SetOrderAmount(orderAmount);
        

        purchaseOrder.SetOrderAmount(newOrderAmount);


        Assert.Equal(
            newOrderAmount,
            purchaseOrder.OrderAmount);

        Assert.Equal(
            expectedDepositAmount,
            purchaseOrder.DepositAmount);
        
        Assert.Equal(
            expectedBalanceAmount,
            purchaseOrder.BalanceAmount);
    }

    [Fact]
    public void SetOrderAmount_AfterDepositPaid_KeepOriginalDeposit()
    {
        decimal depositPercentage = 0.25m;
        decimal orderAmount = 100.00m;
        var depositPaidDate = CreateTestDate();
        decimal newOrderAmount = 150.00m;
        decimal expectedDepositAmount = 25.00m;
        decimal expectedBalanceAmount = 125.00m;

        var paymentTerm = CreatePaymentTerm();
        paymentTerm.SetDepositPercentage(depositPercentage);

        var supplier = CreateSupplier();
        supplier.SetDefaultPaymentTerm(paymentTerm);

        var purchaseOrder = new PurchaseOrder(
            "32",
            PurchaseOrderType.Regular,
            supplier);

        purchaseOrder.SetOrderAmount(orderAmount);
        purchaseOrder.MarkDepositPaid(depositPaidDate);

    
        purchaseOrder.SetOrderAmount(newOrderAmount);


        Assert.True(purchaseOrder.DepositPaid);

        Assert.Equal(
            PaymentStatus.DepositPaid,
            purchaseOrder.PaymentStatus);

        Assert.Equal(
            depositPaidDate,
            purchaseOrder.DepositPaidDate);
        
        Assert.Equal(
            expectedDepositAmount,
            purchaseOrder.DepositAmount);
        
        Assert.Equal(
            expectedBalanceAmount,
            purchaseOrder.BalanceAmount);
    }

    [Fact]
    public void SetOrderAmount_WithFullDeposit_SetBalanceToZero()
    {
        decimal depositPercentage = 1.00m;
        decimal orderAmount = 32.00m;
        decimal expectedDepositAmount = 32.00m;
        decimal expectedBalanceAmount = 00.00m;

        var paymentTerm = CreatePaymentTerm();
        paymentTerm.SetDepositPercentage(depositPercentage);

        var supplier = CreateSupplier();
        supplier.SetDefaultPaymentTerm(paymentTerm);

        var purchaseOrder = new PurchaseOrder(
            "32",
            PurchaseOrderType.Regular,
            supplier);


        purchaseOrder.SetOrderAmount(orderAmount);


        Assert.Equal(
            expectedDepositAmount,
            purchaseOrder.DepositAmount);

        Assert.Equal(
            expectedBalanceAmount,
            purchaseOrder.BalanceAmount);
    }

    [Fact]
    public void SetOrderAmount_WithInvalidAmount_KeepPreviousAmounts()
    {
        decimal depositPercentage = 0.50m;
        decimal orderAmount = 100.00m;
        decimal expectedDepositAmount = 50.00m;
        decimal expectedBalanceAmount = 50.00m;
        decimal invalidOrderAmount = -32.00m;

        var paymentTerm = CreatePaymentTerm();
        paymentTerm.SetDepositPercentage(depositPercentage);

        var supplier = CreateSupplier();
        supplier.SetDefaultPaymentTerm(paymentTerm);

        var purchaseOrder = new PurchaseOrder(
            "32",
            PurchaseOrderType.Regular,
            supplier);
        purchaseOrder.SetOrderAmount(orderAmount);


        Action act = () => purchaseOrder
            .SetOrderAmount(invalidOrderAmount);


        Assert.Throws<DomainException>(act);
        Assert.Equal(
            expectedDepositAmount,
            purchaseOrder.DepositAmount);

        Assert.Equal(
            expectedBalanceAmount,
            purchaseOrder.BalanceAmount);
    }

    [Fact]
    public void MarkDepositPaid_WithoutRequiredDeposit_ThrowDomainException()
    {
        var testDate = CreateTestDate();
        var purchaseOrder = CreatePurchaseOrder();

        Action act = () => purchaseOrder
            .MarkDepositPaid(testDate);
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void MarkDepositPaid_WithRequiredDeposit_UpdatePaymentStates()
    {
        var testDate = CreateTestDate();

        var paymentTerm = CreatePaymentTerm();
        paymentTerm.SetDepositPercentage(0.32m);

        var supplier = CreateSupplier();
        supplier.SetDefaultPaymentTerm(paymentTerm);

        var purchaseOrder = CreatePurchaseOrder(supplier);
        purchaseOrder.SetOrderAmount(32);


        purchaseOrder.MarkDepositPaid(testDate);


        Assert.True(purchaseOrder.DepositPaid);
        Assert.Equal(
            testDate,
            purchaseOrder.DepositPaidDate);
        
        Assert.Equal(
            PaymentStatus.DepositPaid,
            purchaseOrder.PaymentStatus);
    }

    [Fact]
    public void MarkDepositPaid_WhenAlreadyPaid_ThrowDomainException()
    {
        var testDate = CreateTestDate();

        var paymentTerm = CreatePaymentTerm();
        paymentTerm.SetDepositPercentage(0.32m);

        var supplier = CreateSupplier();
        supplier.SetDefaultPaymentTerm(paymentTerm);

        var purchaseOrder = CreatePurchaseOrder(supplier);


        purchaseOrder.MarkDepositPaid(testDate);
        Action act = () => purchaseOrder
            .MarkDepositPaid(testDate);


        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void MarkDepositPaid_AfterOrderPaid_ThrowDomainException()
    {
        var depositTestDate = CreateTestDate();
        var balanceTestDate = CreateTestDate().AddDays(32);

        var paymentTerm = CreatePaymentTerm();
        paymentTerm.SetDepositPercentage(0.32m);

        var supplier = CreateSupplier(paymentTerm);

        var purchaseOrder = CreatePurchaseOrder(supplier);
        purchaseOrder.MarkDepositPaid(depositTestDate);
        purchaseOrder.MarkOrderPaid(balanceTestDate);


        Action act = () => purchaseOrder
            .MarkDepositPaid(balanceTestDate.AddDays(32));


        Assert.Throws<DomainException>(act);

        Assert.Equal(
            depositTestDate,
            purchaseOrder.DepositPaidDate);
    }

    [Fact]
    public void MarkOrderPaid_WithoutRequiredDeposit_MarkOrderPaid()
    {
        decimal depositPercentage = 0.00m;
        var testDate = CreateTestDate();

        var paymentTerm = CreatePaymentTerm(depositPercentage);
        var supplier = CreateSupplier(paymentTerm);
        var purchaseOrder = CreatePurchaseOrder(supplier);


        purchaseOrder.MarkOrderPaid(testDate);


        Assert.Null(purchaseOrder.DepositPaidDate);
        Assert.False(purchaseOrder.RequiresDeposit);
        Assert.False(purchaseOrder.DepositPaid);
        
        Assert.True(purchaseOrder.OrderPaid);
        Assert.Equal(
            testDate,
            purchaseOrder.BalancePaidDate);
        Assert.Equal(
            PaymentStatus.Paid,
            purchaseOrder.PaymentStatus);
    }

    [Fact]
    public void MarkOrderPaid_WithUnpaidDeposit_ThrowDomainException()
    {
        decimal depositPercentage = 0.32m;
        var testDate = CreateTestDate();

        var paymentTerm = CreatePaymentTerm(depositPercentage);
        var supplier = CreateSupplier(paymentTerm);
        var purchaseOrder = CreatePurchaseOrder(supplier);


        Action act = () => purchaseOrder
            .MarkOrderPaid(testDate);

        
        Assert.Throws<DomainException>(act);

        Assert.Null(purchaseOrder.DepositPaidDate);
        Assert.False(purchaseOrder.DepositPaid);
        Assert.True(purchaseOrder.RequiresDeposit);
        Assert.Equal(
            PaymentStatus.UnPaid,
            purchaseOrder.PaymentStatus);

        Assert.Null(purchaseOrder.BalancePaidDate);
        Assert.False(purchaseOrder.OrderPaid);
    }

    [Fact]
    public void MarkOrderPaid_AfterDepositPaid_MarkOrderPaid()
    {
        decimal depositPercentage = 0.32m;
        var depositTestDate = CreateTestDate();
        var balanceTestDate = CreateTestDate().AddDays(32);

        var paymentTerm = CreatePaymentTerm(depositPercentage);
        var supplier = CreateSupplier(paymentTerm);
        var purchaseOrder = CreatePurchaseOrder(supplier);


        purchaseOrder.MarkDepositPaid(depositTestDate);
        purchaseOrder.MarkOrderPaid(balanceTestDate);


        Assert.True(purchaseOrder.DepositPaid);
        Assert.True(purchaseOrder.RequiresDeposit);
        Assert.Equal(
            depositTestDate,
            purchaseOrder.DepositPaidDate);
        
        Assert.True(purchaseOrder.OrderPaid);
        Assert.Equal(
            PaymentStatus.Paid,
            purchaseOrder.PaymentStatus);
        Assert.Equal(
            balanceTestDate,
            purchaseOrder.BalancePaidDate);
    }
}