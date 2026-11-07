namespace ProcHub.Application.Features.Articles.ArticleGroups.Create;

public sealed record CreateArticleGroupCommand
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}