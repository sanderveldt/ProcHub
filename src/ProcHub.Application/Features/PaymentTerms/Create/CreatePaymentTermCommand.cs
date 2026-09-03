using ProcHub.Domain.Suppliers;

namespace ProcHub.Application.Features.PaymentTerms.Create;
public sealed record CreatePaymentTermCommand(
        string Description,
        decimal DepositPercentage,
        PaymentTerm.PaymentTimings PaymentTiming,
        PaymentTerm.PaymentDateReference DueDateReference,
        int BalanceDueDays
        );
    