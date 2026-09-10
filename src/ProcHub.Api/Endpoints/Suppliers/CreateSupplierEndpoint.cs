using ProcHub.Contracts.Suppliers.Responses;
using ProcHub.Contracts.Suppliers.Requests;
using ProcHub.Application.Features.Suppliers.Create;

using Microsoft.AspNetCore.Http.HttpResults;

namespace ProcHub.Api.Endpoints.Suppliers;

public static class CreateSupplierEndpoint
{
    public static RouteGroupBuilder MapCreateSupplierEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync)
             .WithName("CreateSupplier");
        
        return group;
    }
    
    private static async Task<Created<SupplierResponse>> HandleAsync(
        CreateSupplierRequest request,
        CreateSupplierHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateSupplierCommand(
            request.Code,
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

        return TypedResults.Created(
            $"/api/suppliers/{response.Code}",
            response);
    }
}