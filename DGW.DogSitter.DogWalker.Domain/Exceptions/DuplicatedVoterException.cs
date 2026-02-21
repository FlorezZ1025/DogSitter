namespace DGW.DogSitter.DogWalker.Domain.Exceptions;

public class DuplicatedVoterException : CoreBusinessException
{
    public DuplicatedVoterException(string message) : base(message) { }

    public DuplicatedVoterException(string message, Exception inner) : base(message, inner) { }
}