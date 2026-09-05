using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.PaymentTerms;

public sealed record PaymentTermResult(
    int Id,
    string Description,
    decimal DepositPercentage,
    PaymentTerm.PaymentTimings PaymentTiming,
    PaymentTerm.PaymentDateReference DueDateReference,
    int BalanceDueDays);