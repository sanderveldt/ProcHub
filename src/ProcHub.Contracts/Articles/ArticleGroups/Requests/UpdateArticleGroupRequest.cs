namespace ProcHub.Contracts.Articles.ArticleGroups.Requests;

public sealed record UpdateArticleGroupRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}