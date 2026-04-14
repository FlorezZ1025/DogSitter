using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Application.Mensaje.Queries
{
    public class GetUsuariosFromYecidQueryHandler(
        IMessageService messageService,
        ILogger<GetUsuariosFromYecidQueryHandler> logger) : IRequestHandler<GetUsuariosFromYecidQuery, JsonNode>
    {
        public async Task<JsonNode> Handle(GetUsuariosFromYecidQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Handling GetUsuariosFromYecidQuery -- {Guid.NewGuid()}");
            return await messageService.ProbarApiYecid();
        }
    }
}
