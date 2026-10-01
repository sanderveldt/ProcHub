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
        group.MapDashboardListEndpoint();
        group.MapUpdatePurchaseOrderDatesEndpoint();

        // SuperUser or Admin
        group.MapDeletePurchaseOrderEndpoint();

        return endpoints;
    }
}