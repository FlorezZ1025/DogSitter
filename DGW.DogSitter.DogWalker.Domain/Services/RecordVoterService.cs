using DGW.DogSitter.DogWalker.Domain.Entities;
using DGW.DogSitter.DogWalker.Domain.Ports;
using DGW.DogSitter.DogWalker.Domain.Exceptions;

namespace DGW.DogSitter.DogWalker.Domain.Services;

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
            throw new DuplicatedVoterException("The voter already exists");
        }
    }
}
