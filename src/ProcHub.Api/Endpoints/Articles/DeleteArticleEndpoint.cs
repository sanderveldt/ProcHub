using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Articles.Delete;

namespace ProcHub.Api.Endpoints.Articles;

public static class DeleteArticleEndpoint
{
    public static RouteGroupBuilder MapDeleteArticleEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapDelete("/{code}", HandleAsync)
             .WithName("DeleteArticle");
        
        return group;
    }

    private static async Task<NoContent> HandleAsync(
        string code,
        DeleteArticleHhandler handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(code, cancellationToken);

        return TypedResults.NoContent();
    }
}