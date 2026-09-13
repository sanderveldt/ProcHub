namespace ProcHub.Contracts.ShippingTerms.Requests;

public sealed record ShippingTermRequest(
    int Id,
    string Name,
    string Description);