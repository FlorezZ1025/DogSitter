using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Application.Mensaje.Commands
{
    public class SendMensajeToYecidCommandHandler(
        IMessageService messageService,
        ICuidadorRepository cuidadorRepository,
        ILogger<SendMensajeToYecidCommandHandler> logger)
        : IRequestHandler<SendMensajeToYecidCommand, JsonNode>
    {
        public async Task<JsonNode> Handle(SendMensajeToYecidCommand request, CancellationToken cancellationToken)
        {
            var cualquierCuidador = await cuidadorRepository.GetAllCuidadoresAsync();
            var cuidador = cualquierCuidador.FirstOrDefault() ?? throw new NotFoundEntityException("No se encontró ningún cuidador.");
            var dto = cuidador.ToResponseDto();
            logger.LogInformation("Enviando mensaje a Yecid con los datos del cuidador: {@Nombre}", dto.nombre);
            var respuestaMensaje = await messageService.EnviarMensaje(dto);
            logger.LogInformation("Mensaje enviado a Yecid con respuesta: {@Respuesta}", respuestaMensaje);
            return respuestaMensaje;
        }
    }
}
