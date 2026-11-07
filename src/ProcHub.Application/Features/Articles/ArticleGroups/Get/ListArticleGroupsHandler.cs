using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;

namespace ProcHub.Application.Features.Articles.ArticleGroups.Get;

public sealed class ListArticleGroupsHandler(
    IProcHubDbContext dbContext)
{
    public async Task<List<ArticleGroupResult>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ArticleGroups
            .AsNoTracking()
            .OrderBy(ag => ag.Code)
            .Select(ag => new ArticleGroupResult
            {
                Id = ag.Id,
                Code = ag.Code,
                Name = ag.Name,
                Description = ag.Description
            })
            .ToListAsync(cancellationToken);
    }
}