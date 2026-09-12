using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Contracts.Suppliers.Requests;
using ProcHub.Contracts.Suppliers.Responses;
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
            
            ShippingTimeDays = request.ShippingTimeDays,
            ProductionTimeDays = request.ProductionTimeDays,
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