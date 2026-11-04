using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;
using ProcHub.Domain.Countries;

namespace ProcHub.Application.Features.Countries.Create;

public sealed class CreateCountryHandler(
    IProcHubDbContext dbContext,
    IValidator<CreateCountryCommand> validator)
{
    public async Task<CountryResult> HandleAsync(
        CreateCountryCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Code);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Name);

        var code = command.Code.Trim();
        var name = command.Name.Trim();

        await validator.ValidateAndThrowAsync(
            command, 
            cancellationToken: cancellationToken);
        
        var existingCountry = await dbContext.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.Code == code || c.Name == name,
                cancellationToken);

        if (existingCountry is not null)
        {
            throw new DuplicateResourceException(
                existingCountry.Code == code
                    ? $"A country with code '{code}' already exists."
                    : $"A country with name '{name}' already exists.");
        }

        var country = new Country(code, name);

        dbContext.Countries.Add(country);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CountryResult
        {
            Id = country.Id,
            Code = country.Code,
            Name = country.Name
        };
    }
}
