namespace ProcHub.Application.Features.ShippingTerms.Get;

public sealed record ShippingTermListItem(
    int Id,
    string Name,
    string Description);