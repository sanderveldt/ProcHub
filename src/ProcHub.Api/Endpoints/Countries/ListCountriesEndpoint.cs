using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Countries.Get;
using ProcHub.Contracts.Countries;

namespace ProcHub.Api.Endpoints.Countries;

public static class ListCountriesEndpoint
{
    public static RouteGroupBuilder MapListCountriesEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/", HandleAsync)
             .WithName("ListCountries");
        
        return group;
    }

    private static async Task<Ok<List<CountryResponse>>> HandleAsync(
        ListCountriesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler
            .HandleAsync(cancellationToken);
        
        var response = result
            .Select(c => new CountryResponse
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name
            })
            .ToList();
        
        return TypedResults.Ok(response);
    }
}