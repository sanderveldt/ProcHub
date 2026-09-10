using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Contracts.Suppliers.Responses;
using ProcHub.Application.Features.Suppliers.Get;

namespace ProcHub.Api.Endpoints.Suppliers;

public static class GetSupplierEndpoint
{
    public static RouteGroupBuilder MapGetSupplierEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{code}", HandleAsync)
             .WithName("GetSupplier");

        return group;
    }

    private static async Task<Ok<SupplierResponse>> HandleAsync(
        string code,
        GetSupplierHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetSupplierQuery(code);

        var result = await handler.HandleAsync(
            query,
            cancellationToken);
        
        var response = new SupplierResponse(
            result.Code,
            result.Name,
            result.DefaultPaymentTermDescription,

            result.FullName,

            result.MainShippingTermName,
            result.SecondaryShippingTermName,
            result.SampleShippingTermName,

            result.SecondaryPaymentTermDescription,

            result.MainLeadTimeDays,
            result.ProductionTimeDays,
            result.MainLeadTimeDays,
            result.SecondaryLeadTimeDays,
            result.SampleLeadTimeDays,

            result.Status.ToString(),
            result.CreationDate);
        
        return TypedResults.Ok(response);
    }
}