using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Tests.Suppliers;

public class SupplierGetTermsTests
{
    private static Supplier CreateSupplier()
    {
        return new Supplier(
            "0001",
            "Test Supplier",
            CreatePaymentTerm(),
            CreateShippingTerm());
    }

    private static PaymentTerm CreatePaymentTerm()
    {
        return new PaymentTerm(
            "Test PaymentTerm",
            0.10m,
            PaymentTimings.On,
            PaymentDateReference.OrderDate,
            60);
    }

    private static ShippingTerm CreateShippingTerm()
    {
        return new ShippingTerm(
            "TestTerm", 
            "JustForTesting");
    }

    [Fact]
    public void GetTerms_WhenRegular_ReturnDefaultTerms()
    {
        var supplier = CreateSupplier();
        supplier.SetMainLeadTimeDays(32);

        var terms = supplier
            .GetTerms(PurchaseOrderType.Regular);

        Assert.Same(
            supplier.DefaultPaymentTerm,
            terms.PaymentTerm);
        
        Assert.Same(
            supplier.MainShippingTerm,
            terms.ShippingTerm);
        
        Assert.Equal(32, terms.LeadTimeDays);
    }

    [Fact]
    public void GetTerms_WhenRegularWithOtherTerms_ReturnDefaultTerms()
    {
        var supplier = CreateSupplier();
        supplier.SetMainLeadTimeDays(32);

        supplier.SetSecondaryPaymentTerm(CreatePaymentTerm());
        supplier.SetSecondaryShippingTerm(CreateShippingTerm());
        supplier.SetSecondaryLeadTimeDays(3);

        supplier.SetSamplePaymentTerm(CreatePaymentTerm());
        supplier.SetSampleShippingTerm(CreateShippingTerm());
        supplier.SetSampleLeadTimeDays(2);
        // Each CreateTerm() call creates a different object instance
        // Assert.Same checks for object instance match

        var terms = supplier
            .GetTerms(PurchaseOrderType.Regular);
        
        Assert.Same(
            supplier.DefaultPaymentTerm,
            terms.PaymentTerm);
        
        Assert.Same(
            supplier.MainShippingTerm, 
            terms.ShippingTerm);
        
        Assert.Equal(32, terms.LeadTimeDays);
    }

    [Fact]
    public void GetTerms_WhenExpress_ReturnSecondaryTerms()
    {
        var supplier = CreateSupplier();
        var paymentTerm = CreatePaymentTerm();
        var shippingTerm = CreateShippingTerm();

        supplier.SetSecondaryPaymentTerm(paymentTerm);
        supplier.SetSecondaryShippingTerm(shippingTerm);
        supplier.SetSecondaryLeadTimeDays(32);


        var terms = supplier
            .GetTerms(PurchaseOrderType.Express);

        
        Assert.Same(
            paymentTerm,
            terms.PaymentTerm);
        
        Assert.Same(
            shippingTerm,
            terms.ShippingTerm);
        
        Assert.Equal(32, terms.LeadTimeDays);
    }

    [Fact]
    public void GetTerms_WhenSample_ReturnSampleTerms()
    {
        var supplier = CreateSupplier();
        var paymentTerm = CreatePaymentTerm();
        var shippingTerm = CreateShippingTerm();

        supplier.SetSamplePaymentTerm(paymentTerm);
        supplier.SetSampleShippingTerm(shippingTerm);
        supplier.SetSampleLeadTimeDays(32);


        var terms = supplier
            .GetTerms(PurchaseOrderType.Sample);

        
        Assert.Same(
            paymentTerm,
            terms.PaymentTerm);
        
        Assert.Same(
            shippingTerm,
            terms.ShippingTerm);
        
        Assert.Equal(32, terms.LeadTimeDays);
    }

    [Fact]
    public void GetTerms_WhenExpressWithoutSecondaryTerms_ReturnDefaultTerms()
    {
        var supplier = CreateSupplier();
        supplier.SetMainLeadTimeDays(32);

        var terms = supplier
            .GetTerms(PurchaseOrderType.Express);

        Assert.Same(
            supplier.DefaultPaymentTerm,
            terms.PaymentTerm);
        
        Assert.Same(
            supplier.MainShippingTerm,
            terms.ShippingTerm);
        
        Assert.Equal(32, terms.LeadTimeDays);
    }

    [Fact]
    public void GetTerms_WhenSampleWithoutSampleTerms_ReturnDefaultTerms()
    {
        var supplier = CreateSupplier();
        supplier.SetMainLeadTimeDays(32);

        var terms = supplier
            .GetTerms(PurchaseOrderType.Sample);

        Assert.Same(
            supplier.DefaultPaymentTerm,
            terms.PaymentTerm);
        
        Assert.Same(
            supplier.MainShippingTerm,
            terms.ShippingTerm);
        
        Assert.Equal(32, terms.LeadTimeDays);
    }

    [Fact]
    public void GetTerms_WhenExpressWithPartialTerms_ReturnCorrectTerms()
    {
        var supplier = CreateSupplier();
        var paymentTerm = CreatePaymentTerm();

        supplier.SetMainLeadTimeDays(32);
        supplier.SetSecondaryPaymentTerm(paymentTerm);

        var terms = supplier
            .GetTerms(PurchaseOrderType.Express);
        
        Assert.Same(
            paymentTerm, 
            terms.PaymentTerm);

        Assert.Same(
            supplier.MainShippingTerm,
            terms.ShippingTerm);
        
        Assert.Equal(32, terms.LeadTimeDays);
    }

    [Fact]
    public void GetTerms_WhenSampleWithPartialTerms_ReturnCorrectTerms()
    {
        var supplier = CreateSupplier();
        var shippingTerm = CreateShippingTerm();

        supplier.SetMainLeadTimeDays(32);
        supplier.SetSampleShippingTerm(shippingTerm);
        supplier.SetSampleLeadTimeDays(0);

        var terms = supplier
            .GetTerms(PurchaseOrderType.Sample);
        
        Assert.Same(
            supplier.DefaultPaymentTerm, 
            terms.PaymentTerm);

        Assert.Same(
            shippingTerm,
            terms.ShippingTerm);
        
        Assert.Equal(0, terms.LeadTimeDays);
    }

    [Fact]
    public void GetTerms_WhenOrderTypeInvalid_ThrowDomainException()
    {
        var supplier = CreateSupplier();
        var invalidOrderType = (PurchaseOrderType)32;

        Action act = () => supplier
            .GetTerms(invalidOrderType);
        
        Assert.Throws<DomainException>(act);
    }
}