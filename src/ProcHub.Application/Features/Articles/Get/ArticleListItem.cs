namespace ProcHub.Application.Features.Articles.Get;

public sealed record ArticleListItem
{
    public required string Code { get; init; }
    public required string Description { get; init; }
    public required decimal PurchasePrice { get; init; }
    public required string OrderCode { get; init; }
    public required string EanCode { get; init; }
    public required string ArticleGroupCode { get; init; }
    public required string SupplierCode { get; init; }
}