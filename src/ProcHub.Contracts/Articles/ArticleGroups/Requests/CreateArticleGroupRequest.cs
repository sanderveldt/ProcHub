namespace ProcHub.Contracts.Articles.ArticleGroups.Requests;

public sealed record CreateArticleGroupRequest
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}