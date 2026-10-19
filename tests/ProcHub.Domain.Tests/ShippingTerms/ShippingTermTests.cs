using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Tests.ShippingTerms;

public class ShippingTermTests
{
    private static ShippingTerm CreateShippingTerm()
    {
        return new ShippingTerm(
            "FOC",
            "Free of Charge");
    }

    [Fact]
    public void SetName_IsValid_SetsName()
    {
        var shippingTerm = CreateShippingTerm();
        string name = "TestName";

        shippingTerm.SetName(name);

        Assert.Equal(
            name,
            shippingTerm.Name);
    }

    [Fact]
    public void SetName_WithOutsideWhitespace_TrimsName()
    {
        var shippingTerm = CreateShippingTerm();
        string name = "  Test  ";

        shippingTerm.SetName(name);
        string trimmedName = "Test";

        Assert.Equal(
            trimmedName,
            shippingTerm.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("     ")]
    public void SetName_WhenNullOrWhitespace_ThrowDomainException(
        string? name)
    {
        var shippingTerm = CreateShippingTerm();

        Action act = () => shippingTerm
            .SetName(name!);
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void SetName_WhenAtMaxLength_SetsName()
    {
        var shippingTerm = CreateShippingTerm();
        var name = new string(
            'x',
            ShippingTermConstants.NameMaxLength);
        
        shippingTerm.SetName(name);

        Assert.Equal(
            name,
            shippingTerm.Name);
    }

    [Fact]
    public void SetName_WhenOverMaxLength_ThrowDomainException()
    {
        var shippingTerm = CreateShippingTerm();
        var name = new string(
            'x',
            ShippingTermConstants.NameMaxLength + 1);

        Action act = () => shippingTerm
            .SetName(name);
        
        Assert.Throws<DomainException>(act);
    }


    [Fact]
    public void SetDescription_IsValid_SetsDescription()
    {
        var shippingTerm = CreateShippingTerm();
        string description = "TestDescription";

        shippingTerm.SetDescription(description);

        Assert.Equal(
            description, 
            shippingTerm.Description);
    }

    [Fact]
    public void SetDescription_WithOutsideWhitespace_TrimsDescription()
    {
        var shippingTerm = CreateShippingTerm();
        string description = "  TestDescription  ";

        shippingTerm.SetDescription(description);
        string trimmedDescription = "TestDescription";

        Assert.Equal(
            trimmedDescription,
            shippingTerm.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("     ")]
    public void SetDescription_WhenNullOrWhitespace_ThrowDomainException(
        string? description)
    {
        var shippingTerm = CreateShippingTerm();

        Action act = () => shippingTerm
            .SetDescription(description!);
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void SetDescription_WhenAtMaxLength_SetsDescription()
    {
        var shippingTerm = CreateShippingTerm();
        var description = new string(
            'x',
            ShippingTermConstants.DescriptionMaxLength);
        
        shippingTerm.SetDescription(description);

        Assert.Equal(
            description,
            shippingTerm.Description);
    }

    [Fact]
    public void SetDescription_WhenOverMaxLength_ThrowDomainException()
    {
        var shippingTerm = CreateShippingTerm();
        var description = new string(
            'x',
            ShippingTermConstants.DescriptionMaxLength + 1);
        
        Action act = () => shippingTerm
            .SetDescription(description);
        
        Assert.Throws<DomainException>(act);
    }
}