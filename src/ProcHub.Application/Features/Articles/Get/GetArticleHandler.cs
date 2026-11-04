using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Articles.Get;

public sealed class GetArticleHandler(
    IProcHubDbContext dbContext)
{
    public async Task<ArticleResult> HandleAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        code = code.Trim();

        var article = await dbContext.Articles
            .AsNoTracking()
            .Where(a => a.Code == code)
            .Select(a => new ArticleResult
            {
                Id = a.Id,
                Code = a.Code,
                Description = a.Description,
                PurchasePrice = a.PurchasePrice,
                OrderCode = a.OrderCode,
                EanCode = a.EanCode,
                ArticleGroupId = a.ArticleGroupId,
                ArticleGroupCode = a.ArticleGroup.Code,
                ArticleGroupName = a.ArticleGroup.Name,
                SupplierId = a.SupplierId,
                SupplierCode = a.Supplier.Name,
                SupplierName = a.Supplier.Name,
                Status = a.Status,
                CreationDate = a.CreationDate,
                Version = a.Version
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(
                $"Article with code '{code}' not found");
        
        return article;
    }
}