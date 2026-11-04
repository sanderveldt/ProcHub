using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Countries.Get;

public sealed class GetCountryHandler(
    IProcHubDbContext dbContext)
{
    public async Task<CountryResult> HandleAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ValidationException(
                "Country id cannot be 0 or below.");
        }

        var country = await dbContext.Countries
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CountryResult
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(
                $"Country with id {id} not found");

        return country;
    }
}