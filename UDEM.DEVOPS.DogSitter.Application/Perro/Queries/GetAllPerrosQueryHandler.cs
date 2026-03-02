using MediatR;
using Microsoft.Extensions.Logging;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Application.Perro.Queries
{
    public class GetAllPerrosQueryHandler(
        IPerroRepository perroRepository,
        IRazaRepository razaRepository,
        ICuidadorRepository cuidadorRepository,
        ILogger<GetAllPerrosQueryHandler> _logger) : IRequestHandler<GetAllPerrosQuery, IEnumerable<PerroDto>>
    {
        const string TRAZA = "Se obtuvieron todos los perros";
        public async Task<IEnumerable<PerroDto>> Handle(GetAllPerrosQuery request, CancellationToken cancellationToken)
        {
            var perros = await perroRepository.GetAllAsync();

            var razas = await razaRepository.GetAllRazasAync();
            var cuidadores = await cuidadorRepository.GetAllCuidadoresAsync();

            var razasDict = razas.ToDictionary(r => r.Id);
            var cuidadoresDict = cuidadores.ToDictionary(c => c.Id);

            var dtos = perros.Select(p => {
                var dto = p.ToResponseDto();
                dto.raza = razasDict.TryGetValue(p.razaId, out var raza) ? raza.ToResponseDto() : null;
                dto.cuidador = cuidadoresDict.TryGetValue(p.cuidadorId, out var cuidador) ? cuidador.ToResponseDto() : null;
                return dto;
            });
            _logger.LogInformation(message: TRAZA);
            return dtos;
        }
    }
}
