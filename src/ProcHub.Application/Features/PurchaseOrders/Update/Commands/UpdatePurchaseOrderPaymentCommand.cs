namespace ProcHub.Application.Features.PurchaseOrders.Update;

public sealed record UpdatePurchaseOrderPaymentCommand(
    string Code)
{
    public decimal? OrderAmount { get; init; }
    
    public bool? DepositPaid { get; init; }
    public DateOnly? DepositPaidDate { get; init; }
    
    public bool OrderPaid { get; init; }
    public DateOnly? BalancePaidDate { get; init; }
}