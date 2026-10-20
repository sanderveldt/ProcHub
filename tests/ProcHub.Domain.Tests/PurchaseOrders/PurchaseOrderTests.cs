using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Tests.PurchaseOrders;

public class PurchaseOrderTests
{
    private static PurchaseOrder CreatePurchaseOrder()
    {
        return new PurchaseOrder(
            "PO-32",
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

    private static PaymentTerm CreatePaymentTerm()
    {
        return new PaymentTerm(
            "TestTerm",
            0.00m,
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

        
        Assert.False(purchaseOrder.RequiresDeposit);

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
}