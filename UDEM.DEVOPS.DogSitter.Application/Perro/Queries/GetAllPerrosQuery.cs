using MediatR;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Application.Perro.Queries
{
    public record GetAllPerrosQuery() : IRequest<IEnumerable<PerroDto>>;
}
