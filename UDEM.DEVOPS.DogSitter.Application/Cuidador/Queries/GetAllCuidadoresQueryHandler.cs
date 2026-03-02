using MediatR;
using Microsoft.Extensions.Logging;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Application.Cuidador.Queries
{
    public class GetAllCuidadoresQueryHandler(ICuidadorRepository _repository,
                                   ILogger<GetCuidadorQuery> _logger) : IRequestHandler<GetAllCuidadoresQuery, IEnumerable<CuidadorDto>>
    {
        const string TRAZA = "Se obtuvieron todos los cuidadores";

        public async Task<IEnumerable<CuidadorDto>> Handle(GetAllCuidadoresQuery request, CancellationToken cancellationToken)
        {
           var cuidadores = await _repository.GetAllCuidadoresAsync() 
                                        ?? throw new NotFoundEntityException($"No hay cuidadores registrados");
            var dtos = cuidadores.Select(c => c.ToResponseDto());
            _logger.LogInformation(message: TRAZA);
            return dtos;
        }
    }
}
