using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Countries;
using ProcHub.Application.Features.Countries.Delete;

namespace ProcHub.Api.Endpoints.Countries;

public static class DeleteCountryEndpoint
{
    public static RouteGroupBuilder MapDeleteCountryEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", HandleAsync)
             .WithName("DeleteCountry");

        return group;
    }

    private static async Task<NoContent> HandleAsync(
        int id,
        DeleteCountryHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(
            id,
            cancellationToken);
        
        return TypedResults.NoContent();
    }
}