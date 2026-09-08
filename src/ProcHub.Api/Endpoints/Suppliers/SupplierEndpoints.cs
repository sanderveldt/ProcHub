namespace ProcHub.Api.Endpoints.Suppliers;

public static class SupplierEndpoints
{
    public static IEndpointRouteBuilder MapSupplierEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/suppliers")
            .WithTags("Suppliers");
        
        group.MapCreateSupplierEndpoint();

        return endpoints;
    }
}