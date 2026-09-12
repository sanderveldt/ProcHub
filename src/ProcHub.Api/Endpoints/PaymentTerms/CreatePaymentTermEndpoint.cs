using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.PaymentTerms.Create;
using ProcHub.Contracts.PaymentTerms.Responses;
using ProcHub.Contracts.PaymentTerms.Requests;
using ProcHub.Contracts.PaymentTerms.Enums;

namespace ProcHub.Api.Endpoints.PaymentTerms;

public static class CreatePaymentTermEndpoint
{
    public static RouteGroupBuilder MapCreatePaymentTermEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync)
             .WithName("CreatePaymentTerm");

        return group;
    }

    private static async Task<Created<PaymentTermResponse>> HandleAsync(
        CreatePaymentTermRequest request,
        CreatePaymentTermHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreatePaymentTermCommand(
            request.Description,
            request.DepositPercentage,
            (int)request.PaymentTiming,
            (int)request.DueDateReference,
            request.BalanceDueDays);
        
        var result = await handler.HandleAsync(
            command,
            cancellationToken);
        
        var response = new PaymentTermResponse(
            result.Id,
            result.Description,
            result.DepositPercentage,
            (PaymentTiming)result.PaymentTiming,
            (PaymentDateReference)result.DueDateReference,
            result.BalanceDueDays);
        
        return TypedResults.Created(
            $"/api/payment-terms/{response.Id}",
            response);
    }
}