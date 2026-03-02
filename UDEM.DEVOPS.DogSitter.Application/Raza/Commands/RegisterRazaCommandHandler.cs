using MediatR;
using Microsoft.Extensions.Logging;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Raza;

namespace UDEM.DEVOPS.DogSitter.Application.Raza.Commands
{
    public class RegisterRazaCommandHandler(RegisterRazaService _service, IUnitOfWork unitOfWork, ILogger<RegisterRazaCommandHandler> _logger ) : IRequestHandler<RegisterRazaCommand, RazaDto>
    {
        public const string TRAZA = "Se editó el cuidador {request.dto.Id}";
        public async Task<RazaDto> Handle(RegisterRazaCommand request, CancellationToken cancellationToken)
        {

            var entity = request.dto.ToEntity();
            var razaId = await _service.RegisterRazaAsync(entity);
            await unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation(TRAZA, razaId);
            return entity.ToResponseDto();
        }
    }
}
