using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Articles.ArticleGroups.Get;

public sealed class GetArticleGroupHandler(
    IProcHubDbContext dbContext)
{
    public async Task<ArticleGroupResult> HandleAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        code = code.Trim();

        var articleGroup = await dbContext.ArticleGroups
            .AsNoTracking()
            .Where(ag => ag.Code == code)
            .Select(ag => new ArticleGroupResult
            {
                Id = ag.Id,
                Code = ag.Code,
                Name = ag.Name,
                Description = ag.Description
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(
                $"ArticleGroup with code '{code}' not found.");

        return articleGroup;
    }
}