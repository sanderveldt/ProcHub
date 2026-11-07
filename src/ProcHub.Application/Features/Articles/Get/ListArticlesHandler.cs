using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.Articles.Get;

public sealed class ListArticlesHandler(
    IProcHubDbContext dbContext)
{
    public async Task<List<ArticleListItem>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Articles
            .AsNoTracking()
            .OrderBy(a => a.Code)
            .Select(a => new ArticleListItem
            {
                Code = a.Code,
                Description = a.Description,
                PurchasePrice = a.PurchasePrice,
                OrderCode = a.OrderCode!,
                EanCode = a.EanCode!,
                ArticleGroupCode = a.ArticleGroup.Code,
                SupplierCode = a.Supplier.Code
            })
            .ToListAsync(cancellationToken);
    }
}