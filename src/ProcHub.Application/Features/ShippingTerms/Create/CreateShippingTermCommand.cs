namespace ProcHub.Application.Features.ShippingTerms.Create;

public sealed record CreateShippingTermCommand(
    string Name,
    string Description);