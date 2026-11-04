namespace ProcHub.Application.Features.Countries;

public sealed record CountryResult
{
    public int Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
}