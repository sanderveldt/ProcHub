

using Microsoft.VisualBasic;
using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Suppliers;

public class Supplier
{
    public int Id { get; private set; }
    public DateOnly CreationDate { get; private set; } = 
        DateOnly.FromDateTime(DateTime.Today);
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? FullName { get; private set; }

    public int? MainShippingTermId { get; private set; }
    public ShippingTerm? MainShippingTerm { get; private set; }
    public int? SecondaryShippingTermId { get; private set; }
    public ShippingTerm? SecondaryShippingTerm { get; private set; }
    public int? SampleShippingTermId { get; private set; }
    public ShippingTerm? SampleShippingTerm { get; private set; }
    public int? ShippingTimeDays { get; private set; }
    public int? ProductionTimeDays { get; private set; }

    public int DefaultPaymentTermId { get; private set; }
    public PaymentTerm DefaultPaymentTerm { get; private set; } = null!;
    public int? SecondaryPaymentTermId { get; private set; }
    public PaymentTerm? SecondaryPaymentTerm { get; private set; }

    public int? MainLeadTimedays { get; private set; }
    public int? SecondaryLeadTimeDays { get; private set; }
    public int? SampleLeadTimeDays { get; private set; }

    private Supplier()
    {
    }

    public Supplier(
        string code,
        string name,
        PaymentTerm defaultPaymentTerm)
    {
        SetNumber(code);
        SetName(name);
        SetDefaultPaymentTerm(defaultPaymentTerm);
    }

    public void SetNumber(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new DomainException("Supplier number is required.");
        }

        Code = code.Trim();
    }

    public void SetName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Supplier name is required. ");
        }

        Name = name.Trim();
    }

    public void SetDefaultPaymentTerm(PaymentTerm paymentTerm)
    {
        if (paymentTerm is null)
        {
            throw new DomainException("Payment term is required.");
        }

        DefaultPaymentTerm = paymentTerm;
    }

    public void SetSecondaryPaymentTerm(PaymentTerm paymentTerm)
    {
        ArgumentNullException.ThrowIfNull(paymentTerm);
        SecondaryPaymentTerm = paymentTerm;
    }

    public void SetMainShippingTerm(ShippingTerm shippingTerm)
    {
        MainShippingTerm = shippingTerm;
    }

    public void SetSecondaryShippingTerm(ShippingTerm? shippingTerm)
    {
        SecondaryShippingTerm = shippingTerm;
    }

    public void SetSampleShippingTerm(ShippingTerm? shippingTerm)
    {
        SampleShippingTerm = shippingTerm;
    }

}