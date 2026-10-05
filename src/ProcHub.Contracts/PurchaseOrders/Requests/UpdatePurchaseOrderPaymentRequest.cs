namespace ProcHub.Contracts.PurchaseOrders.Requests;

public sealed record UpdatePurchaseOrderPaymentRequest
{
    public decimal? OrderAmount { get; init; }
    
    public bool DepositPaid { get; init; }
    public DateOnly? DepositPaidDate { get; init; }
    
    public bool OrderPaid { get; init; }
    public DateOnly? BalancePaidDate { get; init; }
}