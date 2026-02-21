using DGW.DogSitter.DogWalker.Domain.Dtos;

namespace DGW.DogSitter.DogWalker.Domain.Ports;
public interface IVoterSimpleQueryRepository
{
    Task<VoterDto> Single(Guid id);
}
