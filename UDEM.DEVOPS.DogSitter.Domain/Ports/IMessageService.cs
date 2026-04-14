using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Domain.Ports
{
    public interface IMessageService
    {
        Task<JsonNode> EnviarMensaje(CuidadorDto cuidador);
        Task<JsonNode> ProbarApiYecid();
    }
}
