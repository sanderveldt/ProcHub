using ProcHub.Contracts.PaymentTerms.Enums;

namespace ProcHub.Contracts.PaymentTerms.Responses;

public sealed record PaymentTermResponse(
    int Id,
    string Description,
    decimal DepositPercentage,
    PaymentTiming PaymentTiming,
    PaymentDateReference DueDateReference,
    int BalanceDueDays);
