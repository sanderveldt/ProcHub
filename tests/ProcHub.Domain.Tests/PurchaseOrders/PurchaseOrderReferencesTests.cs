using ProcHub.Domain.Exceptions;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.Tests.Helpers;

namespace ProcHub.Domain.Tests.PurchaseOrders;

public class PurchaseOrderReferencesTests
{
    [Fact]
    public void SetConfirmationReference_WhenValid_SetReference()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        string testRef = "Test";
        
        purchaseOrder.SetConfirmationReference(testRef);

        Assert.Equal(
            testRef,
            purchaseOrder.ConfirmationReference);
    }

    [Fact]
    public void SetConfirmationReference_WhenAtMaxLength_SetReference()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        var testRef = new string(
            'x',
            PurchaseOrderConstants.ConfirmationRefMaxLength);
        
        purchaseOrder.SetConfirmationReference(testRef);

        Assert.Equal(
            testRef,
            purchaseOrder.ConfirmationReference);
    }

    [Fact]
    public void SetConfirmationReference_WhenOverMaxLength_ThrowDomainException()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        var testRef = new string(
            'x',
            PurchaseOrderConstants.ConfirmationRefMaxLength + 1);

        Action act = () => purchaseOrder
            .SetConfirmationReference(testRef);

        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void SetInvoiceReference_WhenValid_SetReference()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        string testRef = "Test";
        
        purchaseOrder.SetInvoiceReference(testRef);

        Assert.Equal(
            testRef,
            purchaseOrder.InvoiceReference);
    }

    [Fact]
    public void SetInvoiceReference_WhenAtMaxLength_SetReference()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        var testRef = new string(
            'x',
            PurchaseOrderConstants.InvoiceRefMaxLength);
        
        purchaseOrder.SetInvoiceReference(testRef);

        Assert.Equal(
            testRef,
            purchaseOrder.InvoiceReference);
    }

    [Fact]
    public void SetInvoiceReference_WhenOverMaxLength_ThrowDomainException()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        var testRef = new string(
            'x',
            PurchaseOrderConstants.InvoiceRefMaxLength + 1);

        Action act = () => purchaseOrder
            .SetInvoiceReference(testRef);

        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void SetOrderNotes_WhenValid_SetOrderNotes()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        string testRef = "Test";
        
        purchaseOrder.SetOrderNotes(testRef);

        Assert.Equal(
            testRef,
            purchaseOrder.OrderNotes);
    }

    [Fact]
    public void SetOrderNotes_WhenAtMaxLength_SetOrderNotes()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        var testRef = new string(
            'x',
            PurchaseOrderConstants.OrderNotesMaxLength);
        
        purchaseOrder.SetOrderNotes(testRef);

        Assert.Equal(
            testRef,
            purchaseOrder.OrderNotes);
    }

    [Fact]
    public void SetOrderNotes_WhenOverMaxLength_ThrowDomainException()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        var testRef = new string(
            'x',
            PurchaseOrderConstants.OrderNotesMaxLength + 1);

        Action act = () => purchaseOrder
            .SetOrderNotes(testRef);

        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void SetShipmentSize_WhenValid_SetShipmentSize()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        string testRef = "Test";
        
        purchaseOrder.SetShipmentSize(testRef);

        Assert.Equal(
            testRef,
            purchaseOrder.ShipmentSize);
    }

    [Fact]
    public void SetShipmentSize_WhenAtMaxLength_SetShipmentSize()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        var testRef = new string(
            'x',
            PurchaseOrderConstants.ShipmentSizeMaxLength);
        
        purchaseOrder.SetShipmentSize(testRef);

        Assert.Equal(
            testRef,
            purchaseOrder.ShipmentSize);
    }

    [Fact]
    public void SetShipmentSize_WhenOverMaxLength_ThrowDomainException()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        var testRef = new string(
            'x',
            PurchaseOrderConstants.ShipmentSizeMaxLength + 1);

        Action act = () => purchaseOrder
            .SetShipmentSize(testRef);

        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void SetShipmentReference_WhenValid_SetReference()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        string testRef = "Test";
        
        purchaseOrder.SetShipmentReference(testRef);

        Assert.Equal(
            testRef,
            purchaseOrder.ShipmentReference);
    }

    [Fact]
    public void SetShipmentReference_WhenAtMaxLength_SetReference()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        var testRef = new string(
            'x',
            PurchaseOrderConstants.ShipmentRefMaxLength);
        
        purchaseOrder.SetShipmentReference(testRef);

        Assert.Equal(
            testRef,
            purchaseOrder.ShipmentReference);
    }

    [Fact]
    public void SetShipmentReference_WhenOverMaxLength_ThrowDomainException()
    {
        var purchaseOrder = DomainBuilder
            .CreatePurchaseOrder();
        var testRef = new string(
            'x',
            PurchaseOrderConstants.ShipmentRefMaxLength + 1);

        Action act = () => purchaseOrder
            .SetShipmentSize(testRef);

        Assert.Throws<DomainException>(act);
    }
}
