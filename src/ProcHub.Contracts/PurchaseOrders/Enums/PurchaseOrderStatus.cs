using ProcHub.Contracts.PurchaseOrders.Enums;

public enum PurchaseOrderStatus
{
    Drafted = 1,
    ConfirmationPending = 2,
    OrderConfirmed = 3,
    OrderInProduction = 4,
    OrderInspection = 5,
    LoadingPending = 6,
    OrderInTransit = 7,
    WaitingForUnloading = 8,
    ReceivedAtWarehouse = 9,
    ReceivedAtWarehouse2 = 10,
    OrderClosed = 11,
    OrderDiscrepancy = 12,
    Cancelled = 13
}