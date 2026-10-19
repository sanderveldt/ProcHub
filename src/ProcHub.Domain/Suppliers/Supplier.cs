using System.Net.WebSockets;
using ProcHub.Domain.Exceptions;
using ProcHub.Domain.PaymentTerms;
using ProcHub.Domain.PurchaseOrders.Enums;
using ProcHub.Domain.ShippingTerms;

namespace ProcHub.Domain.Suppliers;

public class Supplier
{
    public int Id { get; private set; }
    public DateOnly CreationDate { get; private set; } = 
        DateOnly.FromDateTime(DateTime.Today);
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? FullName { get; private set; }

    public int MainShippingTermId { get; private set; }
    public ShippingTerm MainShippingTerm { get; private set; } = null!;
    public int? SecondaryShippingTermId { get; private set; }
    public ShippingTerm? SecondaryShippingTerm { get; private set; }
    public int? SampleShippingTermId { get; private set; }
    public ShippingTerm? SampleShippingTerm { get; private set; }
    public int? ShippingTimeDays { get; private set; }
    public int? ProductionTimeDays { get; private set; }

    public int DefaultPaymentTermId { get; private set; }
    public PaymentTerm DefaultPaymentTerm { get; private set; } = null!;
    public int? SecondaryPaymentTermId { get; private set; }
    public PaymentTerm? SecondaryPaymentTerm { get; private set; }
    public int? SamplePaymentTermId { get; private set; }
    public PaymentTerm? SamplePaymentTerm { get; private set; }

    public int MainLeadTimeDays { get; private set; } = 0;
    public int? SecondaryLeadTimeDays { get; private set; }
    public int? SampleLeadTimeDays { get; private set; }

    public int? ResponsibleBuyerId { get; private set; }

    public SupplierStatus Status { get; private set; } = SupplierStatus.Active;

    private Supplier()
    {
    }

    public Supplier(
        string code,
        string name,
        PaymentTerm defaultPaymentTerm,
        ShippingTerm mainShippingTerm)
    {
        SetCode(code);
        SetName(name);
        SetDefaultPaymentTerm(defaultPaymentTerm);
        SetMainShippingTerm(mainShippingTerm);
    }

    private void SetCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new DomainException("Supplier code is required.");
        }

        if (code.Length > SupplierConstants.CodeMaxLength)
        {
            throw new DomainException(
                $"Supplier code cannot exceed {SupplierConstants.CodeMaxLength} characters.");
        }

        Code = code.Trim();
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException(
                "Supplier name is required.");
        }

        if (name.Length > SupplierConstants.NameMaxLength)
        {
            throw new DomainException(
                $"Supplier name cannot exceed {SupplierConstants.NameMaxLength} characters.");
        }

        Name = name.Trim();
    }

    public void SetFullName(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            FullName = null;
            return;
        }

        if (fullName.Length > SupplierConstants.FullNameMaxLength)
        {
            throw new DomainException(
                $"Supplier full name cannot exceed {SupplierConstants.FullNameMaxLength} characters.");
        }

        FullName = fullName.Trim();
    }

    public void SetDefaultPaymentTerm(PaymentTerm paymentTerm)
    {
        if (paymentTerm is null)
        {
            throw new DomainException(
                "A Payment term is required.");
        }

        DefaultPaymentTerm = paymentTerm;
    }

    public void SetSecondaryPaymentTerm(PaymentTerm? paymentTerm)
    {
        SecondaryPaymentTerm = paymentTerm;
    }

    public void SetSamplePaymentTerm(PaymentTerm? paymentTerm)
    {
        SamplePaymentTerm = paymentTerm;
    }

    public void SetMainShippingTerm(ShippingTerm shippingTerm)
    {
        if (shippingTerm is null)
        {
            throw new DomainException(
                "A shipping Term is required.");
        }

        MainShippingTerm = shippingTerm;
    }

    public void SetSecondaryShippingTerm(ShippingTerm? shippingTerm)
    {
        SecondaryShippingTerm = shippingTerm;
    }

    public void SetSampleShippingTerm(ShippingTerm? shippingTerm)
    {
        SampleShippingTerm = shippingTerm;
    }

    public void SetShippingTimeDays(int? shippingTimeDays)
    {
        if (shippingTimeDays < 0)
        {
            throw new DomainException(
                "Shipping time days can't be negative.");
        }

        ShippingTimeDays = shippingTimeDays;
    }

    public void SetProductionTimeDays(int? productionTimeDays)
    {
        if (productionTimeDays < 0)
        {
            throw new DomainException(
                "Production time days can't be negative.");
        }

        ProductionTimeDays = productionTimeDays;
    }

    public void SetMainLeadTimeDays(int mainLeadTimeDays)
    {
        if (mainLeadTimeDays < 0)
        {
            throw new DomainException(
                "Main lead time days can't be negative.");
        }

        MainLeadTimeDays = mainLeadTimeDays;
    }

    public void SetSecondaryLeadTimeDays(int? secondaryLeadTimeDays)
    {
        if (secondaryLeadTimeDays < 0)
        {
            throw new DomainException(
                "Secondary lead time days can't be negative.");
        }

        SecondaryLeadTimeDays = secondaryLeadTimeDays;
    }

    public void SetSampleLeadTimeDays(int? sampleLeadTimeDays)
    {
        if (sampleLeadTimeDays < 0)
        {
            throw new DomainException(
                "Sample lead time days can't be negative.");
        }

        SampleLeadTimeDays = sampleLeadTimeDays;
    }

    public void SetResponsibleBuyer(int? userId)
    {
        ResponsibleBuyerId = userId;
    }

    public void Activate()
    {
        Status = SupplierStatus.Active;
    }

    public void Deactivate()
    {
        Status = SupplierStatus.Inactive;
    }


    public SupplierTerms GetTerms(PurchaseOrderType orderType)
    {
        return orderType switch
        {
            PurchaseOrderType.Regular => 
                new SupplierTerms(
                    DefaultPaymentTerm,
                    MainShippingTerm,
                    MainLeadTimeDays),
            
            PurchaseOrderType.Express =>
                new SupplierTerms(
                    SecondaryPaymentTerm ?? DefaultPaymentTerm,
                    SecondaryShippingTerm ?? MainShippingTerm,
                    SecondaryLeadTimeDays ?? MainLeadTimeDays),
                            
            PurchaseOrderType.Sample =>
                new SupplierTerms(
                    SamplePaymentTerm ?? DefaultPaymentTerm,
                    SampleShippingTerm ?? MainShippingTerm,
                    SampleLeadTimeDays ?? MainLeadTimeDays),
            
            _ => throw new DomainException(
                    "This ordertype is not valid.")  
        };
    }

    public sealed record SupplierTerms(
        PaymentTerm PaymentTerm,
        ShippingTerm ShippingTerm,
        int LeadTimeDays);
}