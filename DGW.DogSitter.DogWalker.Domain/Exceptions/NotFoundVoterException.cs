namespace DGW.DogSitter.DogWalker.Domain.Exceptions;

public class NotFoundVoterException : CoreBusinessException
{
    public NotFoundVoterException(string message) : base(message) { }

    public NotFoundVoterException(string message, Exception inner) : base(message, inner) { }
}