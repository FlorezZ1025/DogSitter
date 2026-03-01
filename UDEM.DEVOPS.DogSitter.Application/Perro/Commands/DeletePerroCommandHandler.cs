using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Perro;

namespace UDEM.DEVOPS.DogSitter.Application.Perro.Commands
{
    public class DeletePerroCommandHandler(
        DeletePerroService perroService,
        IUnitOfWork unitOfWork,
        ILogger<DeletePerroCommandHandler> _logger) : IRequestHandler<DeletePerroCommand>
    {
        string TRAZA = "Se eliminó el perro {request.id}";
        public async Task Handle(DeletePerroCommand request, CancellationToken cancellationToken)
        {
            await perroService.DeletePerroAsync(request.Id);
            await unitOfWork.SaveAsync();

            _logger.LogInformation(TRAZA, request.Id);
        }
    }
}
