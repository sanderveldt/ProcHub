namespace ProcHub.Application.Features.Articles.Create;

public sealed record CreateArticleCommand
{
    public required string Code { get; init; }
    public required string Description { get; init; }
    public required decimal PurchasePrice { get; init; }

    public string? OrderCode { get; init; }
    public string? EanCode { get; init; }

    public required int ArticleGroupId { get; init; }
    public required int SupplierId { get; init; }
}