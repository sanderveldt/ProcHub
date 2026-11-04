using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.Articles;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Articles.Create;

public sealed class CreateArticleHandler(
    IProcHubDbContext dbContext,
    IValidator<CreateArticleCommand> validator)
{
    public async Task<ArticleResult> HandleAsync(
        CreateArticleCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken: cancellationToken);

        var code = command.Code.Trim();
        
        var codeExists = await dbContext.Articles
            .AnyAsync(
                a => a.Code == code,
                cancellationToken);
        
        if (codeExists)
        {
            throw new DuplicateResourceException(
                $"An Article with code '{code}' already exists.");
        }


        var articleGroup = await dbContext.ArticleGroups
            .FindAsync(
                [command.ArticleGroupId],
                cancellationToken)
            ?? throw new NotFoundException(
                $"ArticleGroup with id {command.ArticleGroupId} not found.");
        
        var supplier = await dbContext.Suppliers
            .FindAsync(
                [command.SupplierId],
                cancellationToken)
            ?? throw new NotFoundException(
                $"Supplier with id {command.SupplierId} not found.");

        
        var article = new Article(
            code,
            command.Description,
            command.PurchasePrice,
            articleGroup,
            supplier);
        
        dbContext.Articles.Add(article);

        await dbContext.SaveChangesAsync(cancellationToken);

        var result = new ArticleResult
        {
            Id = article.Id,
            Code = article.Code,
            Description = article.Description,
            PurchasePrice = article.PurchasePrice,
            OrderCode = article.OrderCode,
            EanCode = article.EanCode,

            ArticleGroupId = article.ArticleGroupId,
            ArticleGroupCode = article.ArticleGroup.Code,
            ArticleGroupName = article.ArticleGroup.Name,

            SupplierId = article.SupplierId,
            SupplierCode = article.Supplier.Code,
            SupplierName = article.Supplier.Name,

            Status = article.Status,
            CreationDate = article.CreationDate,

            Version = article.Version
        };

        return result;
    }
}