using DGW.DogSitter.DogWalker.Domain.Dtos;
using MediatR;

namespace DGW.DogSitter.DogWalker.Application.Voters;

public record VoterQuery(Guid Uid) : IRequest<VoterDto>;

public record VoterSimpleQuery(Guid Uid) : IRequest<VoterDto>;