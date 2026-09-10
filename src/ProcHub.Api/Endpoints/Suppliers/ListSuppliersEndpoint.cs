using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Suppliers.Get;
using ProcHub.Contracts.Suppliers.Responses;

namespace ProcHub.Api.Endpoints.Suppliers;

public static class ListSuppliersEndpoint
{
    public static RouteGroupBuilder MapListSuppliersEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/", HandleAsync)
             .WithName("ListSuppliers");

        return group;
    }

    private static async Task<Ok<List<SupplierListItemResponse>>> HandleAsync(
        ListSuppliersHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(cancellationToken);

        var response = result
            .Select(s => new SupplierListItemResponse(
                s.Code,
                s.Name,
                s.FullName,
                s.DefaultPaymentTermDescription))
            .ToList();
        
        return TypedResults.Ok(response);
    }
}