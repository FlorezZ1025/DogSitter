using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Cuidador;

namespace UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands
{
    public class DeleteCuidadorCommandHandler(DeleteCuidadorService cuidadorService, IUnitOfWork unitOfWork, ILogger<DeleteCuidadorCommandHandler> _logger) : IRequestHandler<DeleteCuidadorCommand>
    {
        public const string TRAZA = "Se eliminó el cuidador {request.id}";
        public async Task Handle(DeleteCuidadorCommand request, CancellationToken cancellationToken)
        {
            await cuidadorService.DeleteCuidadorAsync(request.id);
            await unitOfWork.SaveAsync();
            _logger.LogInformation(TRAZA, request.id);
        }
    }
}
