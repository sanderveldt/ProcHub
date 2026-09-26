namespace ProcHub.Domain.PurchaseOrders.Enums;

public enum PurchaseOrderStatus
{
    Drafted,
    ConfirmationPending,
    OrderConfirmed,
    OrderInProduction,
    OrderInspection,
    LoadingPending,
    OrderInTransit,
    WaitingForUnloading,
    ReceivedAtWarehouse,
    ReceivedAtWarehouse2,
    OrderClosed,
    OrderDiscrepancy,
    Cancelled
}