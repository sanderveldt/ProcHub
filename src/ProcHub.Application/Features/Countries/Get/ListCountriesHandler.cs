using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Countries.Get;

public sealed class ListCountriesHandler(
        IProcHubDbContext dbContext) 
{
    public async Task<List<CountryResult>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        var countries = await dbContext.Countries
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CountryResult
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name
            })
            .ToListAsync(cancellationToken);
        
        return countries;
    }
}
