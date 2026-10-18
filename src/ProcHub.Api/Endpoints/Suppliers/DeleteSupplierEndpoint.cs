using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Authorization;
using ProcHub.Application.Features.Suppliers.Delete;

namespace ProcHub.Api.Endpoints.Suppliers;

public static class DeleteSupplierEndpoint
{
    public static RouteGroupBuilder MapDeleteSupplierEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapDelete("/{code}", HandleAsync)
             .WithName("DeleteSupplier")
             .RequireAuthorization(
                AppPolicies.SuperUserOrAdmin);
        
        return group;
    }

    private static async Task<NoContent> HandleAsync(
        string code,
        DeleteSupplierHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(code, cancellationToken);

        return TypedResults.NoContent();
    }
}