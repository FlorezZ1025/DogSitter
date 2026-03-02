using MediatR;
using Microsoft.Extensions.Logging;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Application.Raza.Queries
{
    public class GetAllRazasQueryHandler(IRazaRepository _repository,
                                    ILogger<GetAllRazasQuery> _logger) : IRequestHandler<GetAllRazasQuery, IEnumerable<RazaDto>>
    {
        const string TRAZA = "Se obtuvieron todas las razas";

        public async Task<IEnumerable<RazaDto>> Handle(GetAllRazasQuery request, CancellationToken cancellationToken)
        {
            var razas = await _repository.GetAllRazasAync()
                                         ?? throw new NotFoundEntityException($"No hay razas registradas");
            var dtos = razas.Select(r => r.ToResponseDto());
            _logger.LogInformation(message: TRAZA);
            return dtos;
        }
    }
}
