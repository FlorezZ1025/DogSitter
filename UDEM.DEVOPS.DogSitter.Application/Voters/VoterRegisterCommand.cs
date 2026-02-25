using MediatR;

namespace UDEM.DEVOPS.DogSitter.Application.Voters;

public record VoterRegisterCommand(string Nid, string Origin, DateTime Dob) : IRequest<Guid>;

