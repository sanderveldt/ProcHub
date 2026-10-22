using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.Tests.Helpers;

namespace ProcHub.Domain.Tests.PurchaseOrders;

public class PurchaseOrderDueDateTests()
{
    [Theory]
    [InlineData(PaymentTimings.Before, 2032, 9, 30)]
    [InlineData(PaymentTimings.On, 2032, 10, 30)]
    [InlineData(PaymentTimings.After, 2032, 11, 29)]

    public void CalculateDueDate_WithPaymentTimings_CalculateBalanceDueDate(
        PaymentTimings timing,
        int year,
        int month,
        int day)
    {
        int balanceDueDays = 30;
        var testDate = new DateOnly(2032, 10, 30);
        var expectedDueDate = new DateOnly(year, month, day);

        var paymentTerm = DomainBuilder
            .CreatePaymentTerm(
                paymentTiming: timing,
                dateReference: PaymentDateReference.InvoiceDate,
                balanceDueDays: balanceDueDays); 
        
        var supplier = DomainBuilder
            .CreateSupplier(paymentTerm: paymentTerm);
        
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder(supplier: supplier);
        

        purchaseOrder.SetInvoiceDate(testDate);


        Assert.Equal(
            expectedDueDate,
            purchaseOrder.BalanceDueDate);
    }

    [Fact]
    public void SetProductionDoneDate_WhenDueDateReference_CalculateBalanceDueDate()
    {
        var testDate = DomainBuilder.CreateTestDate();

        var paymentTerm = DomainBuilder
            .CreatePaymentTerm(
                paymentTiming: PaymentTimings.On,
                dateReference: PaymentDateReference.ProductionDoneDate);
        
        var supplier = DomainBuilder
            .CreateSupplier(paymentTerm: paymentTerm);
        
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder(supplier: supplier);
        

        purchaseOrder.SetProductionDoneDate(testDate);


        Assert.Equal(
            testDate,
            purchaseOrder.BalanceDueDate);

        Assert.Equal(
            testDate,
            purchaseOrder.ProductionDoneDate);
    }

    [Fact]
    public void SetShippingDate_WhenDueDateReference_CalculateBalanceDueDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var paymentTerm = DomainBuilder
            .CreatePaymentTerm(
                paymentTiming: PaymentTimings.On,
                dateReference: PaymentDateReference.ShippingDate);
        var supplier = DomainBuilder
            .CreateSupplier(paymentTerm: paymentTerm);
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder(supplier: supplier);

        purchaseOrder.SetShippingDate(testDate);

        Assert.Equal(
            testDate,
            purchaseOrder.BalanceDueDate);
        Assert.Equal(
            testDate,
            purchaseOrder.ShippingDate);
    }

    [Fact]
    public void SetInvoiceDate_WhenDueDateReference_CalculateBalanceDueDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var paymentTerm = DomainBuilder
            .CreatePaymentTerm(
                paymentTiming: PaymentTimings.On,
                dateReference: PaymentDateReference.InvoiceDate);
        var supplier = DomainBuilder
            .CreateSupplier(paymentTerm: paymentTerm);
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder(supplier: supplier);

        purchaseOrder.SetInvoiceDate(testDate);

        Assert.Equal(
            testDate,
            purchaseOrder.BalanceDueDate);
        Assert.Equal(
            testDate,
            purchaseOrder.InvoiceDate);
    } 

    [Fact]
    public void SetOrderDate_WhenDueDateReference_CalculateBalanceDueDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var paymentTerm = DomainBuilder
            .CreatePaymentTerm(
                paymentTiming: PaymentTimings.On,
                dateReference: PaymentDateReference.OrderDate);
        var supplier = DomainBuilder
            .CreateSupplier(paymentTerm: paymentTerm);

        // OrderDate property is automatically set to 
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder(supplier: supplier);

        Assert.Equal(
            testDate,
            purchaseOrder.BalanceDueDate);
        Assert.Equal(
            testDate,
            purchaseOrder.OrderDate);
    } 

    [Fact]
    public void SetDeliverydate_WhenDueDateReference_CalculateBalanceDueDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var paymentTerm = DomainBuilder
            .CreatePaymentTerm(
                paymentTiming: PaymentTimings.On,
                dateReference: PaymentDateReference.DeliveryDate);
        var supplier = DomainBuilder
            .CreateSupplier(paymentTerm: paymentTerm);
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder(supplier: supplier);

        purchaseOrder.SetDeliveryDate(testDate);

        Assert.Equal(
            testDate,
            purchaseOrder.BalanceDueDate);
        Assert.Equal(
            testDate,
            purchaseOrder.DeliveryDate);
    } 

    [Fact]
    public void SetBOLdate_WhenDueDateReference_CalculateBalanceDueDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var paymentTerm = DomainBuilder
            .CreatePaymentTerm(
                paymentTiming: PaymentTimings.On,
                dateReference: PaymentDateReference.BillOfLadingDate);
        var supplier = DomainBuilder
            .CreateSupplier(paymentTerm: paymentTerm);
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder(supplier: supplier);

        purchaseOrder.SetBillOfLadingDate(testDate);

        Assert.Equal(
            testDate,
            purchaseOrder.BalanceDueDate);
        Assert.Equal(
            testDate,
            purchaseOrder.BillOfLadingDate);
    } 
}