namespace UDEM.DEVOPS.DogSitter.Domain.Exceptions;

public class NotFoundEntityException : CoreBusinessException
{
    public NotFoundEntityException(string message) : base(message) { }

    public NotFoundEntityException(string message, Exception inner) : base(message, inner) { }
}