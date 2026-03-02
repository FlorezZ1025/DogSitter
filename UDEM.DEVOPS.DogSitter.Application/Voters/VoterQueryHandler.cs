using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;

namespace UDEM.DEVOPS.DogSitter.Application.Voters;

public class VoterQueryHandler(IVoterRepository _repository,
                               ILogger<VoterQueryHandler> _logger) : IRequestHandler<VoterQuery, VoterDto>
{
    const string TRAZA = "Queried voter with id {request.uid}";

    public async Task<VoterDto> Handle(VoterQuery request, CancellationToken cancellationToken)
    {
        var voter = await _repository.ExistsAsync(request.Uid)
                                    ?? throw new NotFoundEntityException($"The voter with the id {request.Uid} does not exist");

        _logger.LogInformation(message: TRAZA, args: request.Uid);

        return new VoterDto(voter.Id, voter.DateOfBirth, voter.Origin);
    }
}
