using ProcHub.Domain.Exceptions;

namespace ProcHub.Domain.Countries;

public class Country
{
    public int Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    private Country()
    {
    }

    public Country(
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
                "Country code is required.");
        }

        if (code.Length > CountryConstants.CodeMaxLength)
        {
            throw new DomainException(
                "Country code cannot exceed " + 
                $"{CountryConstants.CodeMaxLength} characters.");
        }

        Code = code.Trim().ToUpperInvariant();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException(
                "Country name is required.");
        }

        if (name.Length > CountryConstants.NameMaxLength)
        {
            throw new DomainException(
                "Country name cannot exceed " + 
                $"{CountryConstants.NameMaxLength} characters.");
        }

        Name = name.Trim();
    }
}