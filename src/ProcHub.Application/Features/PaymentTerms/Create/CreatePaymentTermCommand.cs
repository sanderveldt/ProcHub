namespace ProcHub.Application.Features.PaymentTerms.Create;
public sealed record CreatePaymentTermCommand(
        string Description,
        decimal DepositPercentage,
        int PaymentTiming,
        int DueDateReference,
        int BalanceDueDays);
    