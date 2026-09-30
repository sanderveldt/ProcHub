using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Tests.PaymentTerms;

public class PaymentTermTests
{
    private static PaymentTerm CreatePaymentTerm()
    {
        return new PaymentTerm(
            "30% Deposit",
            0.30m,
            PaymentTimings.After,
            PaymentDateReference.InvoiceDate,
            60);
    }

    // SetDescription() Tests
    [Fact]
    public void SetDescription_ValidDescription_SetsDescription()
    {
        var paymentTerm = CreatePaymentTerm();
        string description = "TestDescription";

        paymentTerm.SetDescription(description);

        Assert.Equal(
            description, 
            paymentTerm.Description);
    }

    [Fact]
    public void SetDescription_WithOutsideWhitespace_TrimsDescription()
    {
        var paymentTerm = CreatePaymentTerm();
        string description = "  TestDescription  ";

        paymentTerm.SetDescription(description);
        string trimmedDescription = "TestDescription";

        Assert.Equal(
            trimmedDescription,
            paymentTerm.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("     ")]
    public void SetDescription_WhenNullOrWhitespace_ThrowDomainException(
        string? description)
    {
        var paymentTerm = CreatePaymentTerm();

        Action act = () => paymentTerm
            .SetDescription(description!);
        
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void SetDescription_WhenAtMaxLength_SetsDescription()
    {
        var paymentTerm = CreatePaymentTerm();
        var description = new string(
            'x',
            PaymentTermConstants.DescriptionMaxLength);
        
        paymentTerm.SetDescription(description);

        Assert.Equal(
            description,
            paymentTerm.Description);
    }

    [Fact]
    public void SetDescription_WhenMoreThanMaxLength_ThrowDomainException()
    {
        var paymentTerm = CreatePaymentTerm();
        var description = new string(
            'x',
            PaymentTermConstants.DescriptionMaxLength + 1);
        
        Action act = () => paymentTerm
            .SetDescription(description);
        
        Assert.Throws<DomainException>(act);
    }

    // SetDepositPercentage Tests
    [Theory]
    [InlineData(0)]
    [InlineData(0.5)]
    [InlineData(1)]
    public void SetDepositPercentage_WhenValid_UpdatesDepositPercentage(
        decimal depositPercentage)
    {
        var paymentTerm = CreatePaymentTerm();

        paymentTerm.SetDepositPercentage(depositPercentage);

        Assert.Equal(
            depositPercentage,
            paymentTerm.DepositPercentage);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    public void SetDepositPercentage_WhenOutsideRange_ThrowDomainException(
        decimal depositPercentage)
    {
        var paymentTerm = CreatePaymentTerm();

        Action act = () => paymentTerm
            .SetDepositPercentage(depositPercentage);

        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(0.111)]
    [InlineData(0.332)]
    [InlineData(0.999)]
    public void SetDepositPercentage_WhenMoreThan2DecimalPlaces_ThrowDomainException(
        decimal depositPercentage)
    {
        var paymentTerm = CreatePaymentTerm();

        Action act = () => paymentTerm
            .SetDepositPercentage(depositPercentage);
        
        Assert.Throws<DomainException>(act);
    }
}