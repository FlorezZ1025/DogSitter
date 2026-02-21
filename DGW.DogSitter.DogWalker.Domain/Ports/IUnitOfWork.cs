namespace DGW.DogSitter.DogWalker.Domain.Ports;
public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken? cancellationToken = null);
}