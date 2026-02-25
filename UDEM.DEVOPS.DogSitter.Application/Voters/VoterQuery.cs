using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using MediatR;

namespace UDEM.DEVOPS.DogSitter.Application.Voters;

public record VoterQuery(Guid Uid) : IRequest<VoterDto>;

public record VoterSimpleQuery(Guid Uid) : IRequest<VoterDto>;