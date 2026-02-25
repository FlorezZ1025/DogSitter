using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Domain.Ports;
public interface IVoterSimpleQueryRepository
{
    Task<VoterDto> Single(Guid id);
}
