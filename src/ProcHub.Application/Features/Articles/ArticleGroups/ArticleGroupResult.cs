namespace ProcHub.Application.Features.Articles.ArticleGroups;

public sealed record ArticleGroupResult
{
    public int Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}