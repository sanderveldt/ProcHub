namespace ProcHub.Application.Exceptions;

public sealed class InvalidStateException : Exception
{
    public InvalidStateException(string message)
        : base(message)
    {
    }
}