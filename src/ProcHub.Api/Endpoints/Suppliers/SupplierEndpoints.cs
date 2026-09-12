namespace ProcHub.Api.Endpoints.Suppliers;

public static class SupplierEndpoints
{
    public static IEndpointRouteBuilder MapSupplierEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/suppliers")
            .WithTags("Suppliers")
            .RequireAuthorization();
        
        group.MapGetSupplierEndpoint();
        group.MapListSuppliersEndpoint();
        group.MapCreateSupplierEndpoint();
        group.MapUpdateSupplierEndpoint();

        // SuperUser or Admin roles
        group.MapChangeSupplierStatusEndpoint();

        return endpoints;
    }
}