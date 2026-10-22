using ProcHub.Domain.Tests.Helpers;

namespace ProcHub.Domain.Tests.PurchaseOrders;

public class PurchaseOrderDatesTests
{
    [Fact]
    public void SetExpectedDeliveryDate_WithValidDate_SetsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        
        purchaseOrder.SetExpectedDeliveryDate(testDate);

        Assert.Equal(
            testDate,
            purchaseOrder.ExpectedDeliveryDate);
    }

    [Fact]
    public void SetExpectedDeliveryDate_WhenNull_ClearsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.SetExpectedDeliveryDate(testDate);
        purchaseOrder.SetExpectedDeliveryDate(null);

        Assert.Null(purchaseOrder.ExpectedDeliveryDate);
    }

    [Fact]
    public void SetOrderConfirmationDate_WithValidDate_SetsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        
        purchaseOrder.SetOrderConfirmationDate(testDate);

        Assert.Equal(
            testDate,
            purchaseOrder.OrderConfirmationDate);
    }

    [Fact]
    public void SetOrderConfirmationDate_WhenNull_ClearsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.SetOrderConfirmationDate(testDate);
        purchaseOrder.SetOrderConfirmationDate(null);

        Assert.Null(purchaseOrder.OrderConfirmationDate);
    }

    [Fact]
    public void SetRequestedProductionDate_WithValidDate_SetsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        
        purchaseOrder.SetRequestedProductionDate(testDate);

        Assert.Equal(
            testDate,
            purchaseOrder.RequestedProductionDate);
    }

    [Fact]
    public void SetRequestedProductionDate_WhenNull_ClearsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.SetRequestedProductionDate(testDate);
        purchaseOrder.SetRequestedProductionDate(null);

        Assert.Null(purchaseOrder.RequestedProductionDate);
    }

    [Fact]
    public void SetArrivalDate_WithValidDate_SetsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        
        purchaseOrder.SetArrivalDate(testDate);

        Assert.Equal(
            testDate,
            purchaseOrder.ArrivalDate);
    }

    [Fact]
    public void SetArrivalDate_WhenNull_ClearsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.SetArrivalDate(testDate);
        purchaseOrder.SetArrivalDate(null);

        Assert.Null(purchaseOrder.ArrivalDate);
    }

    [Fact]
    public void SetProductionDoneDate_WhenNull_ClearsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.SetProductionDoneDate(testDate);
        purchaseOrder.SetProductionDoneDate(null);

        Assert.Null(purchaseOrder.ProductionDoneDate);
    }

    [Fact]
    public void SetInvoiceDate_WhenNull_ClearsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.SetInvoiceDate(testDate);
        purchaseOrder.SetInvoiceDate(null);

        Assert.Null(purchaseOrder.InvoiceDate);
    }

    [Fact]
    public void SetShippingDate_WhenNull_ClearsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.SetShippingDate(testDate);
        purchaseOrder.SetShippingDate(null);

        Assert.Null(purchaseOrder.ShippingDate);
    }

    [Fact]
    public void SetBillOfLadingDate_WhenNull_ClearsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.SetBillOfLadingDate(testDate);
        purchaseOrder.SetBillOfLadingDate(null);

        Assert.Null(purchaseOrder.BillOfLadingDate);
    }

    [Fact]
    public void SetDeliveryDate_WhenNull_ClearsDate()
    {
        var testDate = DomainBuilder.CreateTestDate();
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.SetDeliveryDate(testDate);
        purchaseOrder.SetDeliveryDate(null);

        Assert.Null(purchaseOrder.DeliveryDate);
    }
}