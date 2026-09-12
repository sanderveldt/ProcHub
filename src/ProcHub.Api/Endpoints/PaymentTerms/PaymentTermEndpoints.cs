namespace ProcHub.Api.Endpoints.PaymentTerms;

public static class PaymentTermEndpoints
{
    public static IEndpointRouteBuilder MapPaymentTermEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/payment-terms")
            .WithTags("PaymentTerms")
            .RequireAuthorization();
        
        group.MapGetPaymentTermEndpoint();
        group.MapListPaymentTermsEndpoint();
        group.MapCreatePaymentTermEndpoint();

        // SuperUser or Admin roles
        group.MapUpdatePaymentTermEndpoint();
        group.MapDeletePaymentTermEndpoint();

        return endpoints;
    }
}