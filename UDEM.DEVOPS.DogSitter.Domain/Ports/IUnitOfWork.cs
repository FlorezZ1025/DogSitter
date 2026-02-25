namespace UDEM.DEVOPS.DogSitter.Domain.Ports;
public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken? cancellationToken = null);
}