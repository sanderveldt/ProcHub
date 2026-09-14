using ProcHub.Domain.PaymentTerms;

namespace ProcHub.Application.Features.PaymentTerms;

public sealed record PaymentTermResult(
    int Id,
    string Description,
    decimal DepositPercentage,
    PaymentTimings PaymentTiming,
    PaymentDateReference DueDateReference,
    int BalanceDueDays);