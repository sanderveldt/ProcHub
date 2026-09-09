using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Api.Endpoints.Suppliers.Requests;
using ProcHub.Api.Endpoints.Suppliers.Responses;
using ProcHub.Application.Features.Suppliers.Update;

namespace ProcHub.Api.Endpoints.Suppliers;

public static class UpdateSupplierEndpoint
{
    public static RouteGroupBuilder MapUpdateSupplierEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPut("/{code}", HandleAsync)
             .WithName("UpdateSupplier");
        
        return group;
    }

    private static async Task<Ok<SupplierResponse>> HandleAsync(
        string code,
        UpdateSupplierRequest request,
        UpdateSupplierHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSupplierCommand(
            request.Name,
            request.DefaultPaymentTermId)
        {
            FullName = request.FullName,

            MainShippingTermId = request.MainShippingTermId,
            SecondaryShippingTermId = request.SecondaryShippingTermId,
            SampleShippingTermId = request.SampleShippingTermId,

            SecondaryPaymentTermId = request.SecondaryPaymentTermId,
            
            MainLeadTimeDays = request.MainLeadTimeDays,
            SecondaryLeadTimeDays = request.SecondaryLeadTimeDays,
            SampleLeadTimeDays = request.SampleLeadTimeDays
        };

        var result = await handler.HandleAsync(
                code,
                command,
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