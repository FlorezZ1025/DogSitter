using MediatR;
using Microsoft.Extensions.Logging;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Raza;

namespace UDEM.DEVOPS.DogSitter.Application.Raza.Commands
{
    public class EditRazaCommandHandler(EditRazaService _service, IUnitOfWork unitOfWork, ILogger<EditRazaCommandHandler> _logger) : IRequestHandler<EditRazaCommand, RazaDto>
    {
        public const string TRAZA = "Se editó la raza {request.dto.Id}";
        public async Task<RazaDto> Handle(EditRazaCommand request, CancellationToken cancellationToken)
        {
            var editedRazaDto = await _service.EditRazaAsync(request.dto);
            await unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation(TRAZA, request.dto.Id);

            return editedRazaDto;
        }
    }
}
