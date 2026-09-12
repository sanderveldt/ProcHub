using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Authorization;
using ProcHub.Application.Features.PaymentTerms.Update;
using ProcHub.Contracts.PaymentTerms.Enums;
using ProcHub.Contracts.PaymentTerms.Requests;
using ProcHub.Contracts.PaymentTerms.Responses;

namespace ProcHub.Api.Endpoints.PaymentTerms;

public static class UpdatePaymentTermEndpoint
{
    public static RouteGroupBuilder MapUpdatePaymentTermEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPut("/{id:int}", HandleAsync)
             .WithName("UpdatePaymentTerm")
             .RequireAuthorization(AppPolicies.SuperUserOrAdmin);
        
        return group;
    }

    private static async Task<Ok<PaymentTermResponse>> HandleAsync(
        int id,
        UpdatePaymentTermRequest request,
        UpdatePaymentTermHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePaymentTermCommand(
            request.Description,
            request.DepositPercentage,
            (int)request.PaymentTiming,
            (int)request.DueDateReference,
            request.BalanceDueDays);
        
        var result = await handler.HandleAsync(
            id,
            command,
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