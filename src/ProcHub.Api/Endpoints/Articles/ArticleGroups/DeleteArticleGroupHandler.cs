using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Articles.ArticleGroups.Delete;

namespace ProcHub.Api.Endpoints.Articles.ArticleGroups;

public static class DeleteArticleGroupEndpoint
{
    public static RouteGroupBuilder MapDeleteArticleGroupEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapDelete("/{code}", HandleAsync)
             .WithName("DeleteArticleGroup");
        
        return group;
    }

    private static async Task<NoContent> HandleAsync(
        string code,
        DeleteArticleGroupHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(code, cancellationToken);

        return TypedResults.NoContent();
    }
}