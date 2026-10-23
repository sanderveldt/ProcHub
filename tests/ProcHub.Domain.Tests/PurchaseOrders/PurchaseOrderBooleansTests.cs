using ProcHub.Domain.Tests.Helpers;

namespace ProcHub.Domain.Tests.PurchaseOrders;

public class PurchaseOrderBooleansTests
{
    [Fact]
    public void MarkConfirmationSent_WhenMarked_SetToTrue()
    {
        bool isSent = true;
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.MarkConfirmationSent(isSent);

        Assert.True(purchaseOrder.OrderConfirmationSent);
    }

    [Fact]
    public void MarkInvoiceSent_WhenMarked_SetToTrue()
    {
        bool isSent = true;
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();

        purchaseOrder.MarkInvoiceSent(isSent);

        Assert.True(purchaseOrder.InvoiceSent);
    }
}