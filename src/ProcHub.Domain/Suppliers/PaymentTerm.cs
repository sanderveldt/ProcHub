namespace ProcHub.Domain.Suppliers;

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
        if(string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Payment term description is required.");
        }

        Description = description.Trim();
    }

    public void SetDepositPercentage(decimal depositPercentage)
    {
        if(depositPercentage is < 0m or > 1m)
        {
            throw new ArgumentOutOfRangeException(
                nameof(depositPercentage),
                "Deposit percentage must be between 0 and 1.");
        }

        DepositPercentage = depositPercentage;
    }

    public void SetPaymentTiming(PaymentTimings paymentTiming)
    {
        PaymentTiming = paymentTiming;
    }

    public void SetDateReference(PaymentDateReference dateReference)
    {
        DueDateReference = dateReference;
    }

    public void SetBalanceDueDays(int balanceDueDays)
    {
        if (int.IsNegative(balanceDueDays))
        {
            throw new ArgumentException("Due days can't be negative.");
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
            
            _ => throw new InvalidOperationException(
                    "Invalid payment timing.")
        };
    }

    public enum PaymentTimings
    {
        On = 0,
        Before = 1,
        After = 2,
    }

    public enum PaymentDateReference
    {
        OrderDate = 0,
        InvoiceDate = 1,
        ShippingDate = 2,
        ArrivalDate = 3,
        BillOfLadingDate = 4,
        ProductionDoneDate = 5
    }

}