using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.PaymentTerms.Get;
using ProcHub.Contracts.PaymentTerms.Responses;

namespace ProcHub.Api.Endpoints.PaymentTerms;

public static class ListPaymentTermsEndpoint
{
    public static RouteGroupBuilder MapListPaymentTermsEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/", HandleAsync)
             .WithName("ListPaymentTerms");

        return group;
    }

    private static async Task<Ok<List<PaymentTermListItemResponse>>> HandleAsync(
        ListPaymentTermsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(cancellationToken);

        var response = result
            .Select(pt => new PaymentTermListItemResponse(
                pt.Id,
                pt.Description))
            .ToList();
        
        return TypedResults.Ok(response);
    }
}