using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProcHub.Domain.Articles.ArticleGroups;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Articles.ArticleGroups.Create;

public sealed class CreateArticleGroupHandler(
    IProcHubDbContext dbContext,
    IValidator<CreateArticleGroupCommand> validator)
{
    public async Task<ArticleGroupResult> HandleAsync(
        CreateArticleGroupCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken: cancellationToken);
        
        var code = command.Code.Trim();

        bool codeExists = await dbContext.ArticleGroups
            .AnyAsync(
                ag => ag.Code == code,
                cancellationToken);
        
        if (codeExists)
        {
            throw new DuplicateResourceException(
                $"An ArticleGroup with code '{code}' already exists.");
        }

        var articleGroup = new ArticleGroup(
            code,
            command.Name);
        
        articleGroup.SetDescription(command.Description);

        dbContext.ArticleGroups.Add(articleGroup);

        await dbContext.SaveChangesAsync(cancellationToken);

        var result = new ArticleGroupResult
        {
            Id = articleGroup.Id,
            Code = articleGroup.Code,
            Name = articleGroup.Name,
            Description = articleGroup.Description
        };

        return result;
    }
}