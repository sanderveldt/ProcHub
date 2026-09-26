namespace ProcHub.Domain.PaymentTerms;

public enum PaymentDateReference
{
    OrderDate = 0,
    ProductionDoneDate = 1,
    InvoiceDate = 2,
    ShippingDate = 3,
    BillOfLadingDate = 4,
    DeliveryDate = 5,
}