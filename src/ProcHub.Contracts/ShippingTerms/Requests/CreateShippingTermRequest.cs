namespace ProcHub.Contracts.ShippingTerms.Requests;

public sealed record CreateShippingTermRequest(
    string Name,
    string Description);