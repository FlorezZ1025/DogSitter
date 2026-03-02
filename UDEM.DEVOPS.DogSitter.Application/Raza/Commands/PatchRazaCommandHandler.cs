using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Raza;

namespace UDEM.DEVOPS.DogSitter.Application.Raza.Commands
{
    public class PatchRazaCommandHandler(EditRazaService razaService, IUnitOfWork unitOfWork, ILogger<PatchRazaCommandHandler> _logger) : IRequestHandler<PatchRazaCommand, RazaDto>
    {
        public const string TRAZA = "Se editó la raza parcialmente {request.dto.Id}";

        public async Task<RazaDto> Handle(PatchRazaCommand request, CancellationToken cancellationToken)
        {
            var editedRazaDto = await razaService.EditRazaAsync(request.dto);
            await unitOfWork.SaveAsync(cancellationToken);
            _logger.LogInformation(TRAZA, request.dto.Id);

            return editedRazaDto;
        }
    }
}
