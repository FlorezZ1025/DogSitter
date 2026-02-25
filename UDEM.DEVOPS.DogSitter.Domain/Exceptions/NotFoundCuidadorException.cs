namespace UDEM.DEVOPS.DogSitter.Domain.Exceptions;

public class NotFoundCuidadorException : CoreBusinessException
{
    public NotFoundCuidadorException(string message) : base(message) { }

    public NotFoundCuidadorException(string message, Exception inner) : base(message, inner) { }
}