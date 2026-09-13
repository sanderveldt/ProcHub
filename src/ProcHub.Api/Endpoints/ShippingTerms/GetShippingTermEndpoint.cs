

using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.ShippingTerms.Get;
using ProcHub.Contracts.ShippingTerms.Responses;

namespace ProcHub.Api.Endpoints.ShippingTerms;

public static class GetShippingTermEndpoint
{
    public static RouteGroupBuilder MapGetShippingTermEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", HandleAsync)
             .WithName("GetShippingTerm");

        return group;
    }

    private static async Task<Ok<ShippingTermResponse>> HandleAsync(
        int id,
        GetShippingTermHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetShippingTermQuery(id);

        var result = await handler.HandleAsync(
            query,
            cancellationToken);
        
        var response = new ShippingTermResponse(
            result.Id,
            result.Name,
            result.Description);
        
        return TypedResults.Ok(response);
    }
}