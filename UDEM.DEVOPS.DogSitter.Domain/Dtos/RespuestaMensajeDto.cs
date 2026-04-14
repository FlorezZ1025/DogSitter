using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Domain.Dtos
{
    public class RespuestaMensajeDto
    {
        private readonly JsonNode? EntidadDeSebas;
        private readonly JsonNode? EntidadDeYecid;
        private readonly JsonNode? EntidadDeSantiago;
    }
}
