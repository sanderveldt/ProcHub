using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Authorization;
using ProcHub.Application.Features.Suppliers.ChangeStatus;
using ProcHub.Contracts.Suppliers.Enums;
using ProcHub.Contracts.Suppliers.Requests;
using ProcHub.Contracts.Suppliers.Responses;

namespace ProcHub.Api.Endpoints.Suppliers;

public static class ChangeSupplierStatusEndpoint
{
    public static RouteGroupBuilder MapChangeSupplierStatusEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPatch("/{code}/status", HandleAsync)
             .WithName("ChangeSupplierStatus")
             .RequireAuthorization(
                AppPolicies.SuperUserOrAdmin);
        
        return group;
    }

    private static async Task<Ok<ChangeSupplierStatusResponse>> HandleAsync(
        string code,
        ChangeSupplierStatusRequest request,
        ChangeSupplierStatusHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ChangeSupplierStatusCommand(
            (int)request.Status);
        
        var status = await handler.HandleAsync(
            code,
            command,
            cancellationToken);
        
        var response = new ChangeSupplierStatusResponse(
            (SupplierStatus)status);
        
        return TypedResults.Ok(response);
    }
}