using FluentValidation;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Articles.Update;

public sealed class UpdateArticleHandler(
    IProcHubDbContext dbContext,
    IValidator<UpdateArticleCommand> validator)
{
    public async Task<uint> HandleAsync(
        string code,
        UpdateArticleCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        code = code.Trim();

        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken: cancellationToken);

        var article = await dbContext.Articles
            .FindAsync(
                [code],
                cancellationToken)
            ?? throw new NotFoundException(
                $"Article with code '{code}' not found. ");

        if (article.Version != command.Version)
        {
            throw new ConcurrencyException(
                $"Article '{article.Code}' was changed by another user.");
        }

        article.SetDescription(command.Description);
        article.SetPurchasePrice(command.PurchasePrice);
        article.SetOrderCode(command.OrderCode);
        article.SetEanCode(command.EanCode);
        article.SetStatus(command.Status);
        
        if (article.ArticleGroupId != command.ArticleGroupId)
        {

            var articleGroup = await dbContext.ArticleGroups
                .FindAsync(
                    [command.ArticleGroupId],
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"ArticleGroup with id '{command.ArticleGroupId}' not found.");
            
            article.SetArticleGroup(articleGroup);
        }

        if (article.SupplierId != command.SupplierId)
        {
            var supplier = await dbContext.Suppliers
                .FindAsync(
                    [command.SupplierId],
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"Supplier with id '{command.SupplierId}' not found.");
            
            article.SetSupplier(supplier);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return article.Version;
    }
}