using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;

namespace UDEM.DEVOPS.DogSitter.Domain.Services;

[DomainService]
public class RecordVoterService
{
    private readonly IVoterRepository _voterRepository;

    public RecordVoterService(IVoterRepository voterRepository) => _voterRepository = voterRepository;

    public async Task RecordVoterAsync(Voter voter)
    {
        if (voter.IsUnderAge)
        {
            throw new UnderAgeException("The voter is underaged");
        }

        if (!voter.CanVoteBasedOnLocation)
        {
            throw new LocationNotAllowedException($"The voter is not allowed to vote in this location {voter.Origin}");
        }

        await CheckIfExistsAsync(voter);

        await _voterRepository.SaveVoterAsync(voter);
    }

    private async Task CheckIfExistsAsync(Voter voter)
    {
        var voterExists = await _voterRepository.ExistsAsync(voter.Id);
        if (voterExists is not null)
        {
            throw new DuplicatedEntityException("The voter already exists");
        }
    }
}
