namespace ProcHub.Domain.PurchaseOrders.Enums;

public enum PurchaseOrderStatus
{
    Drafted = 0,
    ConfirmationPending = 1,
    OrderConfirmed = 2,
    OrderInProduction = 3,
    OrderInspection = 4,
    LoadingPending = 5,
    OrderInTransit = 6,
    WaitingForUnloading = 7,
    ReceivedAtWarehouse = 8,
    ReceivedAtWarehouse2 = 9,
    OrderClosed = 10,
    OrderDiscrepancy = 11,
    Cancelled = 12
}