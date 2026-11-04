namespace ProcHub.Api.Endpoints.Countries;

public  static class CountryEndpoints
{
    public static IEndpointRouteBuilder MapCountryEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/countries")
            .WithTags("Countries")
            .RequireAuthorization();
        
        group.MapCreateCountryEndpoint();
        group.MapGetCountryEndpoint();
        group.MapListCountriesEndpoint();
        group.MapDeleteCountryEndpoint();

        return group;
    }
}