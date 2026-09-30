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