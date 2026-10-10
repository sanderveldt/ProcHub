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

    // Constructor SetCode()
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
    [Fact]
    public void SetFullName_IsValid_SetsFullName()
    {
        var supplier = CreateSupplier();
        string fullName = "TestName";

        supplier.SetFullName(fullName);

        Assert.Equal(
            fullName,
            supplier.FullName);
    }

    [Fact]
    public void SetFullName_WithOutsideWhiteSpace_TrimsFullName()
    {
        var supplier = CreateSupplier();
        string fullName = "  Test Name  ";

        supplier.SetFullName(fullName);
        string trimmedName = "Test Name";

        Assert.Equal(
            trimmedName,
            supplier.FullName);
    }

    [Fact]
    public void SetFullName_WhenWhiteSpace_SetsNull()
    {
        var supplier = CreateSupplier();
        string fullName = "     ";

        supplier.SetFullName(fullName);
        
        Assert.Null(supplier.FullName);
    }

    [Fact]
    public void SetFullName_WhenOverMaxLength_ThrowDomainException()
    {
        var supplier = CreateSupplier();
        var fullName = new string(
            's',
            SupplierConstants.FullNameMaxLength + 1);
        
        Action act = () => supplier
            .SetFullName(fullName);

        Assert.Throws<DomainException>(act);
    }

    // SetDefaultPaymentTerm()
    [Fact]
    public void SetDefaultPaymentTerm_IsValid_UpdatePaymentTerm()
    {
        var supplier = CreateSupplier();
        var paymentTerm = CreatePaymentTerm();

        supplier.SetDefaultPaymentTerm(paymentTerm);

        Assert.Same(
            paymentTerm, 
            supplier.DefaultPaymentTerm);
    }

    [Fact]
    public void SetDefaultPaymentTerm_WhenNull_ThrowDomainException()
    {
        var supplier = CreateSupplier();

        Action act = () => supplier
            .SetDefaultPaymentTerm(null!);
        
        Assert.Throws<DomainException>(act);
    }

    // SetMainShippingTerm()
    [Fact]
    public void SetMainShippingTerm_IsValid_UpdateShippingTerm()
    {
        var supplier = CreateSupplier();
        var shippingTerm = CreateShippingTerm();

        supplier.SetMainShippingTerm(shippingTerm);

        Assert.Same(
            shippingTerm,
            supplier.MainShippingTerm);
    }

    [Fact]
    public void SetMainShippingTerm_WhenNull_ThrowDomainException()
    {
        var supplier = CreateSupplier();

        Action act = () => supplier
            .SetMainShippingTerm(null!);

        Assert.Throws<DomainException>(act);
    }

    // SetShippingTimeDays()
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(32)]
    [InlineData(150)]
    public void SetShippingTimeDays_WhenValid_SetShippingDays(
        int? shippingDays)
    {
        var supplier = CreateSupplier();

        supplier.SetShippingTimeDays(shippingDays);

        Assert.Equal(
            shippingDays,
            supplier.ShippingTimeDays);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-32)]
    [InlineData(int.MinValue)]
    public void SetShippingTimeDays_WhenNegative_ThrowDomainException(
        int? shippingDays)
    {
        var supplier = CreateSupplier();

        Action act = () => supplier
            .SetShippingTimeDays(shippingDays);
        
        Assert.Throws<DomainException>(act);
    }

    // SetProductionTimeDays()
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(32)]
    [InlineData(150)]
    public void SetProductionTimeDays_WhenValid_SetShippingDays(
        int? prodDays)
    {
        var supplier = CreateSupplier();

        supplier.SetProductionTimeDays(prodDays);

        Assert.Equal(
            prodDays,
            supplier.ProductionTimeDays);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-32)]
    [InlineData(int.MinValue)]
    public void SetProductionTimeDays_WhenNegative_ThrowDomainException(
        int? prodDays)
    {
        var supplier = CreateSupplier();

        Action act = () => supplier
            .SetProductionTimeDays(prodDays);
        
        Assert.Throws<DomainException>(act);
    }

    // SetMainLeadTimeDays()
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(32)]
    [InlineData(150)]
    public void SetMainLeadTimeDays_WhenValid_SetShippingDays(
        int leadDays)
    {
        var supplier = CreateSupplier();

        supplier.SetMainLeadTimeDays(leadDays);

        Assert.Equal(
            leadDays,
            supplier.MainLeadTimeDays);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-32)]
    [InlineData(int.MinValue)]
    public void SetMainLeadTimeDays_WhenNegative_ThrowDomainException(
        int leadDays)
    {
        var supplier = CreateSupplier();

        Action act = () => supplier
            .SetMainLeadTimeDays(leadDays);
        
        Assert.Throws<DomainException>(act);
    }

    // SetSecondaryLeadTimeDays()
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(32)]
    [InlineData(150)]
    public void SetSecondaryLeadTimeDays_WhenValid_SetShippingDays(
        int? leadDays)
    {
        var supplier = CreateSupplier();

        supplier.SetSecondaryLeadTimeDays(leadDays);

        Assert.Equal(
            leadDays,
            supplier.SecondaryLeadTimeDays);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-32)]
    [InlineData(int.MinValue)]
    public void SetSecondaryLeadTimeDays_WhenNegative_ThrowDomainException(
        int? leadDays)
    {
        var supplier = CreateSupplier();

        Action act = () => supplier
            .SetSecondaryLeadTimeDays(leadDays);
        
        Assert.Throws<DomainException>(act);
    }

    // SetSampleLeadTimeDays
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(32)]
    [InlineData(150)]
    public void SetSampleLeadTimeDays_WhenValid_SetShippingDays(
        int? leadDays)
    {
        var supplier = CreateSupplier();

        supplier.SetSampleLeadTimeDays(leadDays);

        Assert.Equal(
            leadDays,
            supplier.SampleLeadTimeDays);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-32)]
    [InlineData(int.MinValue)]
    public void SetSampleLeadTimeDays_WhenNegative_ThrowDomainException(
        int? leadDays)
    {
        var supplier = CreateSupplier();

        Action act = () => supplier
            .SetSampleLeadTimeDays(leadDays);
        
        Assert.Throws<DomainException>(act);
    }

    // Activate()
    [Fact]
    public void Activate_WhenInactive_SetStatusToActive()
    {
        var supplier = CreateSupplier();
        supplier.Deactivate();

        supplier.Activate();

        Assert.Equal(
            SupplierStatus.Active, 
            supplier.Status);
    }

    // Deactivate()
    [Fact]
    public void Deactivate_WhenActive_SetStatusToInactive()
    {
        var supplier = CreateSupplier();

        supplier.Deactivate();

        Assert.Equal(
            SupplierStatus.Inactive,
            supplier.Status);
    }
}