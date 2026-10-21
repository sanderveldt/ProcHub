using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.Exceptions;
using ProcHub.Domain.Tests.Helpers;
using Microsoft.VisualBasic;

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
}