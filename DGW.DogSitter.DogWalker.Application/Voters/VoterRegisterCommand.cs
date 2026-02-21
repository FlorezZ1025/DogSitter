using MediatR;

namespace DGW.DogSitter.DogWalker.Application.Voters;

public record VoterRegisterCommand(string Nid, string Origin, DateTime Dob) : IRequest<Guid>;

