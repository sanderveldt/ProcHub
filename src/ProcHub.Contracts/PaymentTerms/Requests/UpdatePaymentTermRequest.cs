using ProcHub.Contracts.PaymentTerms.Enums;

namespace ProcHub.Contracts.PaymentTerms.Requests;

public sealed record UpdatePaymentTermRequest(
    string Description,
    decimal DepositPercentage,
    PaymentTiming PaymentTiming,
    PaymentDateReference DueDateReference,
    int BalanceDueDays);
