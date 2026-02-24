using DGW.DogSitter.DogWalker.Application.Voters;
using DGW.DogSitter.DogWalker.Domain.Dtos;
using DGW.DogSitter.DogWalker.Domain.Exceptions;
using DGW.DogSitter.DogWalker.Domain.Mappings;
using DGW.DogSitter.DogWalker.Domain.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGW.DogSitter.DogWalker.Application.Cuidador.Queries
{
    public class GetCuidadorQueryHandler(ICuidadorRepository _repository,
                                   ILogger<GetCuidadorQuery> _logger) : IRequestHandler<GetCuidadorQuery, CuidadorDto>
    {
        const string TRAZA = "Queried voter with id {request.uid}";

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
