using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Articles.ArticleGroups.Get;
using ProcHub.Contracts.Articles.ArticleGroups.Responses;

namespace ProcHub.Api.Endpoints.Articles.ArticleGroups;

public static class GetArticleGroupEndpoint
{
    public static RouteGroupBuilder MapGetArticleGroupEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{code}", HandleAsync)
             .WithName("GetArticleGroup");
        
        return group;
    }

    private static async Task<Ok<ArticleGroupResponse>> HandleAsync(
        string code,
        GetArticleGroupHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler
            .HandleAsync(
                code,
                cancellationToken);
        
        var response = new ArticleGroupResponse
        {
            Id = result.Id,
            Code = result.Code,
            Name = result.Name,
            Description = result.Description
        };

        return TypedResults.Ok(response);
    }
}