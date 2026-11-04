namespace ProcHub.Application.Features.Countries.Create;

public sealed class CreateCountryCommand(
    string Code,
    string Name)
{
    public string Code { get; } = Code;
    public string Name { get; } = Name;
}

