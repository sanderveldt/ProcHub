namespace ProcHub.Application.Exceptions;

public sealed class ResourceInUseException : Exception
{
    public ResourceInUseException(string message)
        : base(message)
    {
    }
}