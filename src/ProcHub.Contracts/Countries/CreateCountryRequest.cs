namespace ProcHub.Contracts.Countries;

public sealed record CreateCountryRequest
{
    public required string Code { get; init; }
    public required string Name { get; init; }
}