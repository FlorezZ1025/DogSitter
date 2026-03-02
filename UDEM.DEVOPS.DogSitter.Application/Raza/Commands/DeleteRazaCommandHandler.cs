using MediatR;
using Microsoft.Extensions.Logging;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Raza;

namespace UDEM.DEVOPS.DogSitter.Application.Raza.Commands
{
    public class DeleteRazaCommandHandler(DeleteRazaService razaService, IUnitOfWork unitOfWork, ILogger<DeleteRazaCommandHandler> _logger) : IRequestHandler<DeleteRazaCommand>
    {
        public const string TRAZA = "Se eliminó la raza {request.id}";

        public async Task Handle(DeleteRazaCommand request, CancellationToken cancellationToken)
        {
            await razaService.DeleteRazaAsync(request.id);
            await unitOfWork.SaveAsync();

            _logger.LogInformation(TRAZA, request.id);
        }
    }
}
