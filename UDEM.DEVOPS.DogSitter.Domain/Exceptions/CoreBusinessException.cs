namespace UDEM.DEVOPS.DogSitter.Domain.Exceptions;

public class CoreBusinessException : Exception
{
    public CoreBusinessException(string message) : base(message) { }

    public CoreBusinessException(string message, Exception inner) : base(message, inner) { }
}