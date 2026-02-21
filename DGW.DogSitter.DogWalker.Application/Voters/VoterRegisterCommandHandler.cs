using DGW.DogSitter.DogWalker.Domain.Ports;
using DGW.DogSitter.DogWalker.Domain.Services;
using MediatR;
using DGW.DogSitter.DogWalker.Domain.Entities;

namespace DGW.DogSitter.DogWalker.Application.Voters;

public class VoterRegisterCommandHandler(IUnitOfWork _unitOfWork, RecordVoterService _service) : IRequestHandler<VoterRegisterCommand, Guid>
{
    public async Task<Guid> Handle(VoterRegisterCommand request, CancellationToken cancellationToken)
    {
        var (nid, origin, dob) = request;
        var voter = new Voter(nid, dob, origin);
        await _service.RecordVoterAsync(voter);
        await _unitOfWork.SaveAsync(cancellationToken);
        return voter.Id;
    }
}
