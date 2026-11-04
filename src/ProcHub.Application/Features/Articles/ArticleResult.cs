using ProcHub.Domain.Articles;

namespace ProcHub.Application.Features.Articles;

public sealed record ArticleResult
{
    public int Id { get; init; }
    public required string Code { get; init; }
    public required string Description { get; init; }
    public required decimal PurchasePrice { get; init; }
    public string? OrderCode { get; init; }
    public string? EanCode { get; init; }

    public required int ArticleGroupId { get; init; }
    public required string ArticleGroupCode { get; init; }
    public required string ArticleGroupName { get; init; }

    public required int SupplierId { get; init; }
    public required string SupplierCode { get; init; }
    public required string SupplierName { get; init; }

    public required ArticleStatus Status { get; init; }
    public required DateOnly CreationDate { get; init; }

    public uint Version { get; init; }
}