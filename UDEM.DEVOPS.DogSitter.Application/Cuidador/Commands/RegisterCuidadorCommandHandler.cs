using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Cuidador;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands
{
    public class RegisterCuidadorCommandHandler(RegisterCuidadorService _service, IUnitOfWork _unitOfWork, ILogger<RegisterCuidadorCommand> _logger) : IRequestHandler<RegisterCuidadorCommand, Guid>
    {
        public async Task<Guid> Handle(RegisterCuidadorCommand request, CancellationToken cancellationToken)
        {
            var entity = request.dto.ToEntity();
            var cuidadorId = await _service.RegisterCuidadorAsync(entity);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation("Cuidador {CuidadorId} registered successfully.", cuidadorId);

            return cuidadorId;
        }
    }
}
