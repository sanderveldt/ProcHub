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

        // SuperUser and Admin roles
        group.MapUpdatePaymentTermEndpoint();

        // Admin only 
        group.MapDeletePaymentTermEndpoint();

        return endpoints;
    }
}