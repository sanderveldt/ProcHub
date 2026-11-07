using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using ProcHub.Application.Features.Articles.ArticleGroups.Update;
using ProcHub.Contracts.Articles.ArticleGroups.Requests;
using ProcHub.Contracts.Articles.ArticleGroups.Responses;
using ProcHub.Contracts.Articles.Responses;

namespace ProcHub.Api.Endpoints.Articles.ArticleGroups;

public static class UpdateArticleGroupEndpoint
{
    public static RouteGroupBuilder MapUpdateArticleGroupEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPut("/", HandleAsync)
             .WithName("UpdateArticleGroup");
        
        return group;
    }

    private static async Task<Ok<ArticleGroupResponse>> HandleAsync(
        string code,
        UpdateArticleGroupRequest request,
        UpdateArticleGroupHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateArticleGroupCommand
        {
            Name = request.Name,
            Description = request.Description
        };

        var result = await handler
            .HandleAsync(
                code,
                command,
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