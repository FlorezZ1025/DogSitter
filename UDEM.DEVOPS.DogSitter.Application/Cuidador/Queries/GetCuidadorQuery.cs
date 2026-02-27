using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using MediatR;

namespace UDEM.DEVOPS.DogSitter.Application.Cuidador.Queries
{
    public record GetCuidadorQuery(Guid id) : IRequest<CuidadorDto> ;
}
