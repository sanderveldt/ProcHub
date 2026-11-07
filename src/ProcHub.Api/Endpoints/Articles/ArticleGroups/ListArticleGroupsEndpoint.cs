using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Articles.ArticleGroups.Get;
using ProcHub.Contracts.Articles.ArticleGroups.Responses;

namespace ProcHub.Api.Endpoints.Articles.ArticleGroups;

public static class ListArticleGroupsEndpoint
{
    public static RouteGroupBuilder MapListArticleGroupsEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/", HandleAsync)
             .WithName("ListArticleGroups");
        
        return group;
    }

    private static async Task<Ok<List<ArticleGroupResponse>>> HandleAsync(
        ListArticleGroupsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler
            .HandleAsync(cancellationToken);
        
        var response = result
            .Select(ag => new ArticleGroupResponse
            {
                Id = ag.Id,
                Code = ag.Code,
                Name = ag.Name,
                Description = ag.Description
            })
            .ToList();
        
        return TypedResults.Ok(response);
    }
}