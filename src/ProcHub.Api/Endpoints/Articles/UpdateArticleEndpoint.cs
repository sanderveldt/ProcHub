using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Articles.Update;
using ProcHub.Contracts.Articles.Enums;
using ProcHub.Contracts.Articles.Requests;

namespace ProcHub.Api.Endpoints.Articles;

public static class UpdateArticleEndpoint
{
    public static RouteGroupBuilder MapUpdateArticleEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPut("/{code}", HandleAsync)
             .WithName("UpdateArticle");
        
        return group;
    }

    private static async Task<Ok<uint>> HandleAsync(
        string code,
        UpdateArticleRequest request,
        UpdateArticleHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateArticleCommand
        {
            Description = request.Description,
            PurchasePrice = request.PurchasePrice,
            OrderCode = request.OrderCode,
            EanCode = request.EanCode,
            ArticleGroupId = request.ArticleGroupId,
            SupplierId = request.SupplierId,
            Status = (int)request.Status,
            Version = request.Version
        };

        var response = await handler
            .HandleAsync(
                code,
                command,
                cancellationToken);
        
        return TypedResults.Ok(response);
    }
}