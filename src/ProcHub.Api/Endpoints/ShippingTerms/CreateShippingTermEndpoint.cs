using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.ShippingTerms.Create;
using ProcHub.Contracts.ShippingTerms.Requests;
using ProcHub.Contracts.ShippingTerms.Responses;

namespace ProcHub.Api.Endpoints.ShippingTerms;

public static class CreateShippingTermEndpoint
{
    public static RouteGroupBuilder MapCreateShippingTermEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync)
             .WithName("CreateShippingTerm");
            
        return group;
    }

    private static async Task<Created<ShippingTermResponse>> HandleAsync(
        ShippingTermRequest request,
        CreateShippingTermHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateShippingTermCommand(
            request.Name,
            request.Description);
        
        var result = await handler.HandleAsync(
            command,
            cancellationToken);
        
        var response = new ShippingTermResponse(
            result.Id,
            result.Name,
            result.Description);
        
        return TypedResults.Created(
            $"/api/shipping-terms/{response.Id}",
            response);
    }
}