using Microsoft.AspNetCore.Http.HttpResults;
using ProcHub.Application.Features.Articles.Get;
using ProcHub.Contracts.Articles.Responses;

namespace ProcHub.Api.Endpoints.Articles;

public static class ListArticleEndpoint
{
    public static RouteGroupBuilder MapListArticlesEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/", HandleAsync)
             .WithName("ListArticles");
        
        return group;
    }

    private static async Task<Ok<List<ArticleListItemResponse>>> HandleAsync(
        ListArticlesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler
            .HandleAsync(cancellationToken);

        var response = result
            .Select(a => new ArticleListItemResponse
            {
                Code = a.Code,
                Description = a.Description,
                PurchasePrice = a.PurchasePrice,
                OrderCode = a.OrderCode,
                EanCode = a.EanCode,
                ArticleGroupCode = a.ArticleGroupCode,
                SupplierCode = a.SupplierCode
            })
            .ToList();
        
        return TypedResults.Ok(response);
    }
}