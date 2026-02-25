using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using MediatR;

namespace UDEM.DEVOPS.DogSitter.Application.Voters;

public class VoterSimpleQueryHandler(IVoterSimpleQueryRepository repository) : IRequestHandler<VoterSimpleQuery, VoterDto>
{
    private readonly IVoterSimpleQueryRepository _repository = repository;

    public async Task<VoterDto> Handle(VoterSimpleQuery request, CancellationToken cancellationToken)
    {
        return await _repository.Single(request.Uid);
    }
}