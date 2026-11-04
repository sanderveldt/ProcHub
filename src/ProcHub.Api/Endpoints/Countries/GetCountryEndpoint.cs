using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Countries.Get;
using ProcHub.Contracts.Countries;

namespace ProcHub.Api.Endpoints.Countries;

public static class GetCountryEndpoint
{
    public static RouteGroupBuilder MapGetCountryEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", HandleAsync)
             .WithName("GetCountry");

        return group;
    }

    private static async Task<Ok<CountryResponse>> HandleAsync(
        int id,
        GetCountryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            id,
            cancellationToken);
        
        var reponse = new CountryResponse
        {
            Id = result.Id,
            Code = result.Code,
            Name = result.Name
        };

        return TypedResults.Ok(reponse);
    }
}