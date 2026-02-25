namespace UDEM.DEVOPS.DogSitter.Domain.Exceptions;

public class LocationNotAllowedException : CoreBusinessException
{
    public LocationNotAllowedException(string message) : base(message) { }

    public LocationNotAllowedException(string message, Exception inner) : base(message, inner) { }
}