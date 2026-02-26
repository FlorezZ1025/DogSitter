using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands
{
    using global::UDEM.DEVOPS.DogSitter.Domain.Dtos;
    using global::UDEM.DEVOPS.DogSitter.Domain.Ports;
    using global::UDEM.DEVOPS.DogSitter.Domain.Services.Cuidador;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands
    {
        public class PatchCuidadorCommandHandler(EditCuidadorService cuidadorService, IUnitOfWork unitOfWork, ILogger<EditCuidadorCommandHandler> _logger) : IRequestHandler<PatchCuidadorCommand, CuidadorDto>
        {
            public const string TRAZA = "Se editó el cuidador parcialmente {request.dto.Id}";
            public async Task<CuidadorDto> Handle(PatchCuidadorCommand request, CancellationToken cancellationToken)
            {
                var editedCuidadorDto = await cuidadorService.EditCuidadorAsync(request.dto);
                await unitOfWork.SaveAsync();
                _logger.LogInformation(TRAZA, request.dto.Id);

                return editedCuidadorDto;
            }
        }
    }

}
