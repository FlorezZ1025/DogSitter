using UDEM.DEVOPS.DogSitter.Application.Voters;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Application.Cuidador.Queries
{
    public class GetCuidadorQueryHandler(ICuidadorRepository _repository,
                                   ILogger<GetCuidadorQuery> _logger) : IRequestHandler<GetCuidadorQuery, CuidadorDto>
    {
        const string TRAZA = "Se obtuvo el cuidador {request.uid}";

        public async Task<CuidadorDto> Handle(GetCuidadorQuery request, CancellationToken cancellationToken)
        {
            var cuidador = await _repository.GetCuidadorAsync(request.id) 
                                        ?? throw new NotFoundCuidadorException($"El cuidador con el id {request.id} no está registrado");
            var dto = cuidador.ToResponseDto();
            _logger.LogInformation(message: TRAZA, args: request.id);
            return dto;
        }
    }
}
