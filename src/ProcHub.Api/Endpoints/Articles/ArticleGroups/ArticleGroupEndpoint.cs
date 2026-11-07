namespace ProcHub.Api.Endpoints.Articles.ArticleGroups;

public static class ArticleGroupEndpoints
{
    public static IEndpointRouteBuilder MapArticleGroupEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/article-groups")
            .WithTags("ArticleGroups")
            .RequireAuthorization();

        group.MapCreateArticleGroupEndpoint();
        group.MapUpdateArticleGroupEndpoint();
        group.MapGetArticleGroupEndpoint();
        group.MapListArticleGroupsEndpoint();
        group.MapDeleteArticleGroupEndpoint();

        return endpoints;
    }
}