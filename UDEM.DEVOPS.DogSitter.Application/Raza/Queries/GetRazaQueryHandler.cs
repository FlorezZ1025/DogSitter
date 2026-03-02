using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Application.Cuidador.Queries;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Application.Raza.Queries
{
    public class GetRazaQueryHandler(IRazaRepository _repository,
                                   ILogger<GetRazaQuery> _logger) : IRequestHandler<GetRazaQuery, RazaDto>
    {
        const string TRAZA = "Se obtuvo el cuidador {request.uid}";

        public async Task<RazaDto> Handle(GetRazaQuery request, CancellationToken cancellationToken)
        {
            var raza = await _repository.GetRazaAsync(request.id)
                                        ?? throw new NotFoundEntityException($"la raza con el id {request.id} no está registrada");
            var dto = raza.ToResponseDto();
            _logger.LogInformation(message: TRAZA, args: request.id);
            return dto;
        }
    }
}
