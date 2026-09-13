using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.ShippingTerms.Get;
using ProcHub.Contracts.ShippingTerms.Responses;

namespace ProcHub.Api.Endpoints.ShippingTerms;

public static class ListShippingTermEndpoint
{
    public static RouteGroupBuilder MapListShippingTermEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/", HandleAsync)
             .WithName("ListShippingTerms");

        return group;
    }

    private static async Task<Ok<List<ShippingTermResponse>>> HandleAsync(
        ListShippingTermsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(cancellationToken);

        var response = result
            .Select(st => new ShippingTermResponse(
                st.Id,
                st.Name,
                st.Description))
            .ToList();
        
        return TypedResults.Ok(response);
    }
}