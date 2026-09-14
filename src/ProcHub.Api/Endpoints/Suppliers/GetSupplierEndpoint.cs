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

            result.DefaultPaymentTermId,
            result.DefaultPaymentTermDescription,

            result.FullName,

            result.MainShippingTermId,
            result.MainShippingTermName,

            result.SecondaryShippingTermId,
            result.SecondaryShippingTermName,

            result.SampleShippingTermId,
            result.SampleShippingTermName,

            result.SecondaryPaymentTermId,
            result.SecondaryPaymentTermDescription,

            result.ShippingTimeDays,
            result.ProductionTimeDays,
            result.MainLeadTimeDays,
            result.SecondaryLeadTimeDays,
            result.SampleLeadTimeDays,

            result.Status.ToString(),
            result.CreationDate);
        
        return TypedResults.Ok(response);
    }
}