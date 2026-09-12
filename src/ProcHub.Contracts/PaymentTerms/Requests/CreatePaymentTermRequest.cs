using ProcHub.Contracts.PaymentTerms.Enums;

namespace ProcHub.Contracts.PaymentTerms.Requests;

public sealed record CreatePaymentTermRequest(
    string Description,
    decimal DepositPercentage,
    PaymentTiming PaymentTiming,
    PaymentDateReference DueDateReference,
    int BalanceDueDays);
