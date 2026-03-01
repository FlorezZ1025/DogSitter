using MediatR;
using Microsoft.Extensions.Logging;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Perro;

namespace UDEM.DEVOPS.DogSitter.Application.Perro.Commands
{
    public class RegisterPerroCommandHandler(RegisterPerroService perroService, IUnitOfWork unitOfWork, ILogger<RegisterPerroCommandHandler> _logger) : IRequestHandler<RegisterPerroCommand, PerroDto>
    {
        public const string TRAZA = "Se editó el cuidador {request.dto.Id}";
        public async Task<PerroDto> Handle(RegisterPerroCommand request, CancellationToken cancellationToken)
        {
            var entity = request.dto.ToEntity();
            await perroService.RegisterPerroAsync(entity);
            await unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation(TRAZA, entity.Id);
            return entity.ToResponseDto();
        }
    }
}
