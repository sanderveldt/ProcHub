using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Articles.Get;
using ProcHub.Contracts.Articles.Enums;
using ProcHub.Contracts.Articles.Responses;

namespace ProcHub.Api.Endpoints.Articles;

public static class GetArticleEndpoint
{
    public static RouteGroupBuilder MapGetArticleEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{code}", HandleAsync)
             .WithName("GetArticle");

        return group;
    }

    private static async Task<Ok<ArticleResponse>> HandleAsync(
        string code,
        GetArticleHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            code,
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

        return TypedResults.Ok(response);
    }
}