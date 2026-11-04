using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Countries.Create;
using ProcHub.Contracts.Countries;

namespace ProcHub.Api.Endpoints.Countries;

public static class CreateCountryEndpoint
{
    public static RouteGroupBuilder MapCreateCountryEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync)
             .WithName("CreateCountry");

        return group;
    }

    private static async Task<Created<CountryResponse>> HandleAsync(
        CreateCountryRequest request,
        CreateCountryHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateCountryCommand(
            request.Code,
            request.Name);

        var result = await handler.HandleAsync(
            command,
            cancellationToken);
        
        var response = new CountryResponse
        {
            Id = result.Id,
            Code = result.Code,
            Name = result.Name
        };

        return TypedResults.Created(
            $"/api/countries/{response.Id}",
            response);
    }
}