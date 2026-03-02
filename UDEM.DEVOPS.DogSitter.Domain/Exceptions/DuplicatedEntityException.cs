namespace UDEM.DEVOPS.DogSitter.Domain.Exceptions;

public class DuplicatedEntityException : CoreBusinessException
{
    public DuplicatedEntityException(string message) : base(message) { }

    public DuplicatedEntityException(string message, Exception inner) : base(message, inner) { }
}