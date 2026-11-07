namespace ProcHub.Contracts.Articles.ArticleGroups.Responses;

public sealed record ArticleGroupResponse
{
    public int Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}