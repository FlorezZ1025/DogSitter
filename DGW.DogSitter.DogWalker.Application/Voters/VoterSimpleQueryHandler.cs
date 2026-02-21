using DGW.DogSitter.DogWalker.Domain.Dtos;
using DGW.DogSitter.DogWalker.Domain.Ports;
using MediatR;

namespace DGW.DogSitter.DogWalker.Application.Voters;

public class VoterSimpleQueryHandler(IVoterSimpleQueryRepository repository) : IRequestHandler<VoterSimpleQuery, VoterDto>
{
    private readonly IVoterSimpleQueryRepository _repository = repository;

    public async Task<VoterDto> Handle(VoterSimpleQuery request, CancellationToken cancellationToken)
    {
        return await _repository.Single(request.Uid);
    }
}