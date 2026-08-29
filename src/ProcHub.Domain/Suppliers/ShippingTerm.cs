using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Suppliers;

public class ShippingTerm
{
    public int Id { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    private ShippingTerm()
    {
    }

    public ShippingTerm(
        string name,
        string? description)
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

        Name = name.Trim();
    }
    
    public void SetDescription(string? description)
    {
        Description = description;
    }
}