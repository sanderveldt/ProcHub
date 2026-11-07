using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Articles.ArticleGroups.Create;
using ProcHub.Contracts.Articles.ArticleGroups.Requests;
using ProcHub.Contracts.Articles.ArticleGroups.Responses;

namespace ProcHub.Api.Endpoints.Articles.ArticleGroups;

public static class CreateArticleGroupEndpoint
{
    public static RouteGroupBuilder MapCreateArticleGroupEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync)
             .WithName("CreateArticleGroup");
        
        return group;
    }

    private static async Task<Created<ArticleGroupResponse>> HandleAsync(
        CreateArticleGroupRequest request,
        CreateArticleGroupHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateArticleGroupCommand
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description
        };

        var result = await handler
            .HandleAsync(
                command,
                cancellationToken);
        
        var response = new ArticleGroupResponse
        {
            Id = result.Id,
            Code = result.Code,
            Name = result.Name,
            Description = result.Description
        };

        return TypedResults.Created(
            $"/api/article-groups/{response.Code}",
            response);
    }
}
