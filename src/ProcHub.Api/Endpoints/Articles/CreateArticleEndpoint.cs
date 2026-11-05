using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Articles.Create;
using ProcHub.Contracts.Articles.Enums;
using ProcHub.Contracts.Articles.Requests;
using ProcHub.Contracts.Articles.Responses;

namespace ProcHub.Api.Endpoints.Articles;

public static class CreateArticleEndpoint
{
    public static RouteGroupBuilder MapCreateArticleEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync)
             .WithName("CreateArticle");

        return group;
    }

    private static async Task<Created<ArticleResponse>> HandleAsync(
        CreateArticleRequest request,
        CreateArticleHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateArticleCommand
        {
            Code = request.Code,
            Description = request.Description,
            PurchasePrice = request.PurchasePrice,
            OrderCode = request.OrderCode,
            EanCode = request.EanCode,
            ArticleGroupId = request.ArticleGroupId,
            SupplierId = request.SupplierId
        };

        var result = await handler.HandleAsync(
            command,
            cancellationToken);
        
        var response = new ArticleResponse
        {
            Id = result.Id,
            Code = result.Code,
            Description = result.Description,
            PurchasePrice = result.PurchasePrice,
            OrderCode = result.OrderCode,
            EanCode = result.EanCode,

            ArticleGroupId = result.ArticleGroupId,
            ArticleGroupCode = result.ArticleGroupCode,
            ArticleGroupName = result.ArticleGroupName,

            SupplierId = result.SupplierId,
            SupplierCode = result.SupplierCode,
            SupplierName = result.SupplierName,

            Status = (ArticleStatus)result.Status,
            CreationDate = result.CreationDate,

            Version = result.Version
        };

        return TypedResults.Created(
            $"/api/articles/{response.Code}",
            response);
    }
}