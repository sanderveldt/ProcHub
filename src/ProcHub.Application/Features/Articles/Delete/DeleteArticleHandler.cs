using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Articles.Delete;

public sealed class DeleteArticleHhandler(
    IProcHubDbContext dbContext)
{
    public async Task HandleAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ValidationException(
                "Article code cannot be empty");
        }

        var article = await dbContext.Articles
            .FirstOrDefaultAsync(
                a => a.Code == code,
                cancellationToken)
            ?? throw new NotFoundException(
                $"Article with code '{code}' not found.");

        // InUseChecks later?

        dbContext.Articles.Remove(article);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}