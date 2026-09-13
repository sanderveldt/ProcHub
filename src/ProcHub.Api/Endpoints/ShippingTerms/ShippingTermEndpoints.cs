namespace ProcHub.Api.Endpoints.ShippingTerms;

public static class ShippingTermEndpoints
{
    public static IEndpointRouteBuilder MapShippingTermEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/shipping-terms")
            .WithTags("ShippingTerms")
            .RequireAuthorization();
        
        group.MapGetShippingTermEndpoint();
        group.MapListShippingTermEndpoint();
        group.MapCreateShippingTermEndpoint();

        // SuperUser and Admin roles
        group.MapDeleteShippingTermEndpoint();

        return endpoints;
    }
}