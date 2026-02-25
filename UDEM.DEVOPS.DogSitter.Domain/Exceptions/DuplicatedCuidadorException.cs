namespace UDEM.DEVOPS.DogSitter.Domain.Exceptions;

public class DuplicatedCuidadorException : CoreBusinessException
{
    public DuplicatedCuidadorException(string message) : base(message) { }

    public DuplicatedCuidadorException(string message, Exception inner) : base(message, inner) { }
}