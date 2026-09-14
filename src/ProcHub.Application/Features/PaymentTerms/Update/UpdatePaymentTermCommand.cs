namespace ProcHub.Application.Features.PaymentTerms.Update;

public sealed record UpdatePaymentTermCommand(
    string Description,
    decimal DepositPercentage,
    int PaymentTiming,
    int DueDateReference,
    int BalanceDueDays);
