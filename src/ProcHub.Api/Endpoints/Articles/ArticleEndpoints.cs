namespace ProcHub.Api.Endpoints.Articles;

public static class ArticleEndpoints
{
    public static IEndpointRouteBuilder MapArticleEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/articles")
            .WithTags("Articles")
            .RequireAuthorization();

        group.MapCreateArticleEndpoint();
        
        return endpoints;
    }
}