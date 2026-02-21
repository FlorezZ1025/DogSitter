using DGW.DogSitter.DogWalker.Domain.Entities;

namespace DGW.DogSitter.DogWalker.Domain.Ports
{
    public interface IVoterRepository
    {
        Task<Voter> SaveVoterAsync(Voter v);
        Task<Voter?> ExistsAsync(Guid uid);
    }
}

