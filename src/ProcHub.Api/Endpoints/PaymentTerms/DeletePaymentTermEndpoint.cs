using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Authorization;
using ProcHub.Application.Features.PaymentTerms.Delete;

namespace ProcHub.Api.Endpoints.PaymentTerms;

public static class DeletePaymentTermEndpoint
{
    public static RouteGroupBuilder MapDeletePaymentTermEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", HandleAsync)
             .WithName("DeletePaymentTerm")
             .RequireAuthorization(AppPolicies.SuperUserOrAdmin);

        return group;
    }

    private static async Task<NoContent> HandleAsync(
        int id,
        DeletePaymentTermHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(id, cancellationToken);
        
        return TypedResults.NoContent();
    }
}