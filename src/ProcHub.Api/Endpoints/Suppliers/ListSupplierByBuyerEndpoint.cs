using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Suppliers.Get;
using ProcHub.Contracts.Suppliers.Responses;

namespace ProcHub.Api.Endpoints.Suppliers;

public static class ListSuppliersByBuyerEndpoint
{
    public static RouteGroupBuilder MapListSuppliersByBuyerEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/bybuyerid={userId}", HandleAsync)
             .WithName("ListSuppliersByBuyer");

        return group;
    }

    private static async Task<Ok<List<SupplierListItemResponse>>> HandleAsync(
        int userId,
        ListSuppliersByBuyerHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler
            .HandleAsync(
                userId,
                cancellationToken);
        
        var response = result
            .Select(s => new SupplierListItemResponse(
                s.Id,
                s.Code,
                s.Name,
                s.FullName,
                s.DefaultPaymentTermDescription))
            .ToList();
        
        return TypedResults.Ok(response);
    }
}
