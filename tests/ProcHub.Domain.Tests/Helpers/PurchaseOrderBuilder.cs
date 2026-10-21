using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;

namespace ProcHub.Domain.Tests.Helpers;

public class PurchaseOrderBuilder
{
    public static PurchaseOrder CreatePurchaseOrder(
        PurchaseOrderType? orderType = null,
        Supplier? supplier = null)
    {
        return new PurchaseOrder(
            "003200",
            orderType ?? PurchaseOrderType.Regular,
            supplier ?? CreateSupplier());
    }

    public static Supplier CreateSupplier(
        PaymentTerm? paymentTerm = null)
    {
        return new Supplier(
            "32",
            "TestSupplier",
            paymentTerm ?? CreatePaymentTerm(),
            CreateShippingTerm());
    }

    public static PaymentTerm CreatePaymentTerm(
        decimal? depositPercentage = null,
        PaymentTimings? paymentTiming = null,
        PaymentDateReference? dateReference = null,
        int? balanceDueDays = null)
    {
        return new PaymentTerm(
            "TestTerm",
            depositPercentage ?? 0.00m,
            paymentTiming ?? PaymentTimings.After,
            dateReference ?? PaymentDateReference.OrderDate,
            balanceDueDays ?? 32); 
    }

    public static ShippingTerm CreateShippingTerm()
    {
        return new ShippingTerm(
            "TestTerm",
            "Just for Testing");
    }

    public static DateOnly CreateTestDate()
    {
        return DateOnly
            .FromDateTime(DateTime.Today);
    }
}