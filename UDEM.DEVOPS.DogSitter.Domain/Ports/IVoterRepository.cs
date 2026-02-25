using UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Ports
{
    public interface IVoterRepository
    {
        Task<Voter> SaveVoterAsync(Voter v);
        Task<Voter?> ExistsAsync(Guid uid);
    }
}

