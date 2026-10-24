using ProcHub.Domain.Tests.Helpers;

namespace ProcHub.Domain.Tests.PurchaseOrders;

public class PurchaseOrderBooleansTests
{
    [Fact]
    public void MarkConfirmationSent_WhenMarkedFalse_SetToTrue()
    {
        bool isSent = true;
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.MarkConfirmationSent(isSent);

        Assert.True(purchaseOrder.OrderConfirmationSent);
    }

    [Fact]
    public void MarkConfirmationSent_WhenMarkedTrue_SetToFalse()
    {
        bool isSent = false;
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.MarkConfirmationSent(isSent);

        Assert.False(purchaseOrder.OrderConfirmationSent);
    }

    [Fact]
    public void MarkInvoiceSent_WhenMarkedFalse_SetToTrue()
    {
        bool isSent = true;
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.MarkInvoiceSent(isSent);

        Assert.True(purchaseOrder.InvoiceSent);
    }

    [Fact]
    public void MarkInvoiceSent_WhenMarkedTrue_SetToFalse()
    {
        bool isSent = false;
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.MarkInvoiceSent(isSent);

        Assert.False(purchaseOrder.InvoiceSent);
    }

    [Fact]
    public void MarkInspectionDone_WhenMarkedFalse_SetToTrue()
    {
        bool isDone = true;
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.MarkInspectionDone(isDone);

        Assert.True(purchaseOrder.OrderInspectionDone);
    }

    [Fact]
    public void MarkInspectionDone_WhenMarkedTrue_SetToFalse()
    {
        bool isDone = true;
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.MarkInspectionDone(isDone);

        Assert.True(purchaseOrder.OrderInspectionDone);
    }

    [Fact]
    public void MarkCertificateOfOriginSaved_WhenMarkedFalse_SetToTrue()
    {
        bool isSaved = true;
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.MarkCertificateOfOriginSaved(isSaved);

        Assert.True(purchaseOrder.CertificateOfOriginSaved);
    }

    [Fact]
    public void MarkCertificateOfOriginSaved_WhenMarkedTrue_SetToFalse()
    {
        bool isSaved = false;
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.MarkCertificateOfOriginSaved(isSaved);

        Assert.False(purchaseOrder.CertificateOfOriginSaved);
    }
}