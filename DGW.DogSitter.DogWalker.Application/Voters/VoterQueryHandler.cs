using DGW.DogSitter.DogWalker.Domain.Dtos;
using DGW.DogSitter.DogWalker.Domain.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using DGW.DogSitter.DogWalker.Domain.Exceptions;

namespace DGW.DogSitter.DogWalker.Application.Voters;

public class VoterQueryHandler(IVoterRepository _repository,
                               ILogger<VoterQueryHandler> _logger) : IRequestHandler<VoterQuery, VoterDto>
{
    const string TRAZA = "Queried voter with id {request.uid}";

    public async Task<VoterDto> Handle(VoterQuery request, CancellationToken cancellationToken)
    {
        var voter = await _repository.ExistsAsync(request.Uid)
                                    ?? throw new NotFoundCuidadorException($"The voter with the id {request.Uid} does not exist");

        _logger.LogInformation(message: TRAZA, args: request.Uid);

        return new VoterDto(voter.Id, voter.DateOfBirth, voter.Origin);
    }
}
