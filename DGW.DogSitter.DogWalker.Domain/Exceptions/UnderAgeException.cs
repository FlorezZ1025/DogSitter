namespace DGW.DogSitter.DogWalker.Domain.Exceptions;

public class UnderAgeException : CoreBusinessException
{
    public UnderAgeException(string message) : base(message) { }

    public UnderAgeException(string message, Exception inner) : base(message, inner) { }
}