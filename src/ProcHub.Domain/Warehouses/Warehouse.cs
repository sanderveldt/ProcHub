using ProcHub.Domain.Countries;
using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Warehouses;

public class Warehouse
{
    public int Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public int CountryId { get; private set; }
    public Country? Country { get; private set; } = null!;

    private Warehouse()
    {
    }

    public Warehouse(
        string code,
        string name)
    {
        SetCode(code);
        SetName(name);
    }

    private void SetCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new DomainException(
                "Warehouse code is required.");
        }

        if (code.Length > WarehouseConstants.CodeMaxLength)
        {
            throw new DomainException(
                "Warehouse code cannot exceed " +
                $"{WarehouseConstants.CodeMaxLength} characters.");
        }

        Code = code.Trim();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException(
                "Warehouse name is required.");
        }

        if (name.Length > WarehouseConstants.NameMaxLength)
        {
            throw new DomainException(
                "Warehouse name cannot exceed " +
                $"{WarehouseConstants.NameMaxLength} characters.");
        }

        Name = name.Trim();
    }

    public void SetDescription(string description)
    {
        if (description.Length > WarehouseConstants.DescriptionMaxLength)
        {
            throw new DomainException(
                "Warehouse description cannot exceed " +
                $"{WarehouseConstants.DescriptionMaxLength} characters.");
        }

        Description = description.Trim();
    }

    public void SetCity(string city)
    {
        if (city.Length > WarehouseConstants.CityMaxLength)
        {
            throw new DomainException(
                "City name cannot exceed " +
                $"{WarehouseConstants.CityMaxLength} characters. ");
        }

        City = city.Trim();
    }

    public void SetCountry(Country? country)
    {
        Country = country;
    }
}