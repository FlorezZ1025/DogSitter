using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Application.Mensaje.Queries
{
    public class GetUsuariosFromYecidQueryHandler(IMessageService messageService) : IRequestHandler<GetUsuariosFromYecidQuery, JsonNode>
    {
        public async Task<JsonNode> Handle(GetUsuariosFromYecidQuery request, CancellationToken cancellationToken)
        {
            return await messageService.ProbarApiYecid();
        }
    }
}
