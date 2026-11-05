using ProcHub.Domain.Articles;

namespace ProcHub.Application.Features.Articles.Update;

public sealed record UpdateArticleCommand
{
    public required string Description { get; init; }
    public required decimal PurchasePrice { get; init; }
    public string? OrderCode { get; init; }
    public string? EanCode { get; init; }
    public required int ArticleGroupId { get; init; }
    public required int SupplierId { get; init; }
    public required ArticleStatus Status { get; init; }

    public required uint Version { get; init; }
}