using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.PaymentTerms;

public class PaymentTerm
{
    public int Id { get; private set; }

    public string Description { get; private set; } = null!;
    public decimal DepositPercentage { get; private set; }
    public PaymentTimings PaymentTiming { get; private set; }
    public PaymentDateReference DueDateReference { get; private set; }
    public int BalanceDueDays { get; private set; }

    private PaymentTerm()
    {
    }

    public PaymentTerm(
        string description,
        decimal depositPercentage,
        PaymentTimings paymentTiming,
        PaymentDateReference dueDateReference,
        int balanceDueDays)
    {
        SetDescription(description);
        SetDepositPercentage(depositPercentage);
        SetPaymentTiming(paymentTiming);
        SetDateReference(dueDateReference);
        SetBalanceDueDays(balanceDueDays);
    }

    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Payment term description is required.");
        }

        if (description.Length > PaymentTermConstants.DescriptionMaxLength)
        {
            throw new DomainException(
                $"PaymentTerm description cannot exceed {PaymentTermConstants.DescriptionMaxLength} characters.");
        }

        Description = description.Trim();
    }

    public void SetDepositPercentage(decimal depositPercentage)
    {
        if(depositPercentage is < 0m or > 1m)
        {
            throw new DomainException(
                "Deposit percentage must be between 0 and 1.");
        }

        DepositPercentage = depositPercentage;
    }

    public void SetPaymentTiming(PaymentTimings paymentTiming)
    {
        if (!Enum.IsDefined(paymentTiming))
        {
            throw new DomainException(
                $"{paymentTiming} is not a valid PaymentTiming.");
        }

        PaymentTiming = paymentTiming;
    }

    public void SetDateReference(PaymentDateReference dateReference)
    {
        if (!Enum.IsDefined(dateReference))
        {
            throw new DomainException(
                $"{dateReference} is not a valid Payment date reference.");
        }

        DueDateReference = dateReference;
    }

    public void SetBalanceDueDays(int balanceDueDays)
    {
        if (int.IsNegative(balanceDueDays))
        {
            throw new DomainException("Due days can't be negative.");
        }

        BalanceDueDays = balanceDueDays;
    }

    public DateOnly CalculateDueDate(DateOnly referenceDate)
    {
        return PaymentTiming switch
        {
            PaymentTimings.On => referenceDate,

            PaymentTimings.Before => 
                referenceDate.AddDays(-BalanceDueDays),

            PaymentTimings.After =>
                referenceDate.AddDays(BalanceDueDays),
            
            _ => throw new DomainException(
                    "Invalid payment timing.")
        };
    }
}