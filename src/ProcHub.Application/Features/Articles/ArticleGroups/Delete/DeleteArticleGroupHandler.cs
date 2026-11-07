using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Articles.ArticleGroups.Delete;

public sealed class DeleteArticleGroupHandler(
    IProcHubDbContext dbContext)
{
    public async Task HandleAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
       ArgumentException.ThrowIfNullOrWhiteSpace(code);

       var articleGroup = await dbContext.ArticleGroups
            .FirstOrDefaultAsync(
                ag => ag.Code == code,
                cancellationToken)
            ?? throw new NotFoundException(
                $"ArticleGroup with code '{code}' not found.");
        
        bool isInUse = await dbContext.Articles
            .AnyAsync(
                a => a.ArticleGroupId == articleGroup.Id,
                cancellationToken);
            
        if (isInUse)
        {
            throw new ResourceInUseException(
                $"ArticleGroup '{code}' is still attached to an article.");
        }

        dbContext.ArticleGroups.Remove(articleGroup);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}