namespace ProcHub.Contracts.Countries;

public sealed record CountryResponse
{
    public int Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
}