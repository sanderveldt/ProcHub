using ProcHub.Domain.Suppliers;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.ShippingTerms;
using ProcHub.Domain.PurchaseOrders;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.Countries;
using ProcHub.Domain.Articles.ArticleGroups;
using ProcHub.Domain.Articles;

namespace ProcHub.Domain.Tests.Helpers;

public class DomainBuilder
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

    public static Country CreateCountry()
    {
        return new Country(
            "TEST",
            "Test Country");
    }

    public static ArticleGroup CreateArticleGroup()
    {
        return new ArticleGroup(
            "32",
            "TEST");
    }

    public static Article CreateArticle(
        string? code = null,
        decimal? purchasePrice = null,
        ArticleGroup? group = null,
        Supplier? supplier = null)
    {
        return new Article(
            code ?? "032",
            "TestDescription",
            purchasePrice ?? 32.32m,
            group ?? CreateArticleGroup(),
            supplier ?? CreateSupplier());
    }
}