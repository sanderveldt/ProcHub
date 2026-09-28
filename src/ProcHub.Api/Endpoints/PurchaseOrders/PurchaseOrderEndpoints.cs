namespace ProcHub.Api.Endpoints.PurchaseOrders;

public static class PurchaseOrderEndpoints
{
    public static IEndpointRouteBuilder MapPurchaseOrderEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/purchaseorders")
            .WithTags("PurchaseOrders")
            .RequireAuthorization();

        group.MapCreatePurchaseOrderEndpoint();
        group.MapGetPurchaseOrderEndpoint();
        group.MapListPurchaseOrdersEndpoint();

        return endpoints;
    }
}