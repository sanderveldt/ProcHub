namespace ProcHub.Application.Features.Countries.Create;

public sealed record CreateCountryCommand(
    string Code,
    string Name);