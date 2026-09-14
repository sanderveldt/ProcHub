using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.ShippingTerms;

public class ShippingTerm
{
    public int Id { get; private set; }

    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    private ShippingTerm()
    {
    }

    public ShippingTerm(
        string name,
        string description)
    {
        SetName(name);
        SetDescription(description);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Shipping term name is required.");
        }

        if (name.Length > ShippingTermConstants.NameMaxLength)
        {
            throw new DomainException(
                $"ShippingTerm name cannot exceed {ShippingTermConstants.NameMaxLength} characters.");
        }

        Name = name.Trim();
    }
    
    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Shipping term description is required.");
        }

        if (description.Length > ShippingTermConstants.DescriptionMaxLength)
        {
            throw new DomainException(
                $"ShippingTerm description cannot exceed {ShippingTermConstants.DescriptionMaxLength} characters.");
        }

        Description = description;
    }
}