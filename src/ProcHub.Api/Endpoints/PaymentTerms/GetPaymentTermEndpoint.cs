using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.PaymentTerms.Get;
using ProcHub.Contracts.PaymentTerms.Enums;
using ProcHub.Contracts.PaymentTerms.Responses;

namespace ProcHub.Api.Endpoints.PaymentTerms;

public static class GetPaymentTermEndpoint
{
    public static RouteGroupBuilder MapGetPaymentTermEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", HandleAsync)
             .WithName("GetPaymentTerm");
        
        return group;
    }

    private static async Task<Ok<PaymentTermResponse>> HandleAsync(
        int id,
        GetPaymentTermHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentTermQuery(id);

        var result = await handler.HandleAsync(
            query,
            cancellationToken);
        
        var response = new PaymentTermResponse(
            result.Id,
            result.Description,
            result.DepositPercentage,
            (PaymentTiming)result.PaymentTiming,
            (PaymentDateReference)result.DueDateReference,
            result.BalanceDueDays);

        return TypedResults.Ok(response);
    }
}