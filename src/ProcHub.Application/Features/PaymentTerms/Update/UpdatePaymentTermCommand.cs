using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.PaymentTerms.Update;

public sealed record UpdatePaymentTermCommand(
    string Description,
    decimal DepositPercentage,
    PaymentTerm.PaymentTimings PaymentTiming,
    PaymentTerm.PaymentDateReference DueDateReference,
    int BalanceDueDays);
