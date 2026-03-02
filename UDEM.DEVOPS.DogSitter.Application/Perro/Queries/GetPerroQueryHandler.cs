using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Application.Perro.Queries
{
    public class GetPerroQueryHandler(IPerroRepository perroRepository, IRazaRepository razaRepository, ICuidadorRepository cuidadorRepository, ILogger<GetPerroQueryHandler> _logger) : IRequestHandler<GetPerroQuery, PerroDto>
    {
        const string TRAZA = "Se obtuvo el perro con id {id}";
        public async Task<PerroDto> Handle(GetPerroQuery request, CancellationToken cancellationToken)
        {
            var perro = await perroRepository.GetPerroAsync(request.id)         
                ?? throw new NotFoundEntityException($"No se encuentra un perro con el id {request.id}");
            var raza = await razaRepository.GetRazaAsync(perro.razaId)
                ?? throw new NotFoundEntityException($"No se encuentra una raza con el id {perro.razaId}");
            var cuidador = await cuidadorRepository.GetCuidadorAsync(perro.cuidadorId)
                ?? throw new NotFoundEntityException($"No se encuentra un cuidador con el id {perro.cuidadorId}");
          
            var perroDto = perro.ToResponseDto();
            perroDto.raza = raza.ToResponseDto();
            perroDto.cuidador = cuidador.ToResponseDto();

            _logger.LogInformation(TRAZA, request.id);
            return perroDto;

        }
    }
}
