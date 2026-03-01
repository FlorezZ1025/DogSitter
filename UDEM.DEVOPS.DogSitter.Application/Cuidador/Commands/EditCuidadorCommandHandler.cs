using MediatR;
using Microsoft.Extensions.Logging;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Cuidador;

namespace UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands
{
    public class EditCuidadorCommandHandler(EditCuidadorService cuidadorService, IUnitOfWork unitOfWork, ILogger<EditCuidadorCommandHandler> _logger) : IRequestHandler<EditCuidadorCommand, CuidadorDto>
    {
        public const string TRAZA = "Se editó el cuidador {request.dto.Id}";
        public async Task<CuidadorDto> Handle(EditCuidadorCommand request, CancellationToken cancellationToken)
        {
            var editedCuidadorDto = await cuidadorService.EditCuidadorAsync(request.dto);
            await unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation(TRAZA, request.dto.Id);

            return editedCuidadorDto;
        }
    }
}
