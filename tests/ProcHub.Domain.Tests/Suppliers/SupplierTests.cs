using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Tests.Suppliers;

public class SupplierTests
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

    // SetCode()
    [Fact]
    public void Constructor_WithValidCode_SetsCode()
    {
        var supplier = CreateSupplier();

        Assert.Equal(
            "0001",
            supplier.Code);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidCode_ThrowDomainException(
        string? code)
    {
        Action act = () => new Supplier(
            code!,
            "Test Supplier",
            CreatePaymentTerm(),
            CreateShippingTerm());
            
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Constructor_WithCodeOverMaxLength_ThrowDomainException()
    {
        var code = new string(
            'A', 
            SupplierConstants.CodeMaxLength + 1);
        
        Action act = () => new Supplier(
            code,
            "Test Supplier",
            CreatePaymentTerm(),
            CreateShippingTerm());
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Constructor_WithOutsideWhiteSpace_TrimsCode()
    {
        var code = " 0001 ";
        var trimmedCode = "0001";

        var supplier = new Supplier(
            code,
            "Test Supplier",
            CreatePaymentTerm(),
            CreateShippingTerm());

        Assert.Equal(
            trimmedCode, 
            supplier.Code);
    }

    // SetName
    [Fact]
    public void SetName_IsValid_SetsName()
    {
        var supplier = CreateSupplier();
        string name = "TestName";

        supplier.SetName(name);

        Assert.Equal(
            name,
            supplier.Name);
    }

    [Fact]
    public void SetName_WithOutsideWhiteSpace_TrimsName()
    {
        var supplier = CreateSupplier();
        string name = "  TestName  ";

        supplier.SetName(name);
        string trimmedName = "TestName";

        Assert.Equal(
            trimmedName,
            supplier.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("     ")]
    public void SetName_WhenNullOrWhitespace_ThrowDomainException(
        string? name)
    {
        var supplier = CreateSupplier();

        Action act = () => supplier
            .SetName(name!);
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void SetName_WhenOverMaxLength_ThrowDomainException()
    {
        var supplier = CreateSupplier();
        var name = new string(
            'A', 
            SupplierConstants.NameMaxLength + 1);

        Action act = () => supplier
            .SetName(name);
        
        Assert.Throws<DomainException>(act);
    }

    // SetFullName()




}