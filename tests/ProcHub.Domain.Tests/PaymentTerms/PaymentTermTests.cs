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


    // SetDescription()
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


    // SetDepositPercentage()
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


    // SetPaymentTiming()
    [Fact]
    public void SetPaymentTiming_WithInvalidInput_ThrowDomainException()
    {
        var paymentTerm = CreatePaymentTerm();
        var invalidPaymentTiming = (PaymentTimings)999;

        Action act = () => paymentTerm
            .SetPaymentTiming(invalidPaymentTiming);
        
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(PaymentTimings.Before)]
    [InlineData(PaymentTimings.On)]
    [InlineData(PaymentTimings.After)]
    public void SetPaymentTiming_WithValidInput_SetsPaymentTiming(
        PaymentTimings paymentTiming)
    {
        var paymentTerm = CreatePaymentTerm();

        paymentTerm.SetPaymentTiming(paymentTiming);

        Assert.Equal(
            paymentTiming, 
            paymentTerm.PaymentTiming);
    }


    // SetDateReference()
    [Fact]
    public void SetDateReference_WithInvalidInput_ThrowDomainException()
    {
        var paymentTerm = CreatePaymentTerm();
        var invalidDateReference = (PaymentDateReference)999;

        Action act = () => paymentTerm
            .SetDateReference(invalidDateReference);
        
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(PaymentDateReference.BillOfLadingDate)]
    [InlineData(PaymentDateReference.DeliveryDate)]
    [InlineData(PaymentDateReference.InvoiceDate)]
    [InlineData(PaymentDateReference.OrderDate)]
    [InlineData(PaymentDateReference.ProductionDoneDate)]
    [InlineData(PaymentDateReference.ShippingDate)]
    public void SetDateReference_WithValidInput_SetsDateReference(
        PaymentDateReference dateReference)
    {
        var paymentTerm = CreatePaymentTerm();

        paymentTerm.SetDateReference(dateReference);

        Assert.Equal(
            dateReference,
            paymentTerm.DueDateReference);
    }


    // SetBalanceDueDays()
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(32)]
    [InlineData(150)]
    public void SetBalanceDueDays_WithValidInput_SetsBalanceDueDays(
        int balanceDueDays)
    {
        var paymentTerm = CreatePaymentTerm();

        paymentTerm.SetBalanceDueDays(balanceDueDays);

        Assert.Equal(
            balanceDueDays,
            paymentTerm.BalanceDueDays);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-32)]
    [InlineData(int.MinValue)]
    public void SetBalanceDueDays_WhenNegative_ThrowDomainException(
        int balanceDueDays)
    {
        var paymentTerm = CreatePaymentTerm();

        Action act = () => paymentTerm
            .SetBalanceDueDays(balanceDueDays);
        
        Assert.Throws<DomainException>(act);
    }
}