using FluentValidation;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Articles.ArticleGroups.Update;

public sealed class UpdateArticleGroupHandler(
    IProcHubDbContext dbContext,
    IValidator<UpdateArticleGroupCommand> validator)
{
    public async Task<ArticleGroupResult> HandleAsync(
        string code,
        UpdateArticleGroupCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        code = code.Trim();

        await validator.ValidateAndThrowAsync(
            command,
            cancellationToken);
        
        var articleGroup = await dbContext.ArticleGroups
            .FindAsync(
                [code],
                cancellationToken)
            ?? throw new NotFoundException(
                $"ArticleGroup with code '{code}' not found.");
        
        articleGroup.SetName(command.Name);
        articleGroup.SetDescription(command.Description);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new ArticleGroupResult
        {
            Id = articleGroup.Id,
            Code = articleGroup.Code,
            Name = articleGroup.Name,
            Description = articleGroup.Description
        };
    }
}