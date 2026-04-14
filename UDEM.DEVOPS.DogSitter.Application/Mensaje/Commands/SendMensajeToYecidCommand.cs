using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Application.Mensaje.Commands
{
    public record SendMensajeToYecidCommand() : IRequest<JsonNode>;
}
