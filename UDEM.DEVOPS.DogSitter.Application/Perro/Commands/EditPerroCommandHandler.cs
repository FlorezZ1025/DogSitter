using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Perro;

namespace UDEM.DEVOPS.DogSitter.Application.Perro.Commands
{
    public class EditPerroCommandHandler(
        EditPerroService perroService,
        ICuidadorRepository cuidadorRepository,
        IRazaRepository razaRepository,
        IUnitOfWork unitOfWork,
        ILogger<EditPerroCommandHandler> _logger) : IRequestHandler<EditPerroCommand, PerroDto>
    {
        public const string TRAZA = "Se editó el perro {request.dto.Id}";

        public async Task<PerroDto> Handle(EditPerroCommand request, CancellationToken cancellationToken)
        {
            var editedPerroDto = await perroService.EditPerroAsync(request.dto);
            var raza = await razaRepository.GetRazaAsync(editedPerroDto.razaId);
            var cuidador = await cuidadorRepository.GetCuidadorAsync(editedPerroDto.cuidadorId);

            editedPerroDto.raza = raza?.ToResponseDto();
            editedPerroDto.cuidador = cuidador?.ToResponseDto();

            await unitOfWork.SaveAsync();
            _logger.LogInformation(TRAZA, request.dto.Id);

            return editedPerroDto;
        }
    }
}
