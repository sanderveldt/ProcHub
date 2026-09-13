namespace ProcHub.Contracts.ShippingTerms.Responses;

public sealed record ShippingTermResponse(
    int Id,
    string Name,
    string Description);