using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Domain.Ports
{
    public interface IRickAndMortyService
    {
        Task<JsonNode?> GetCharactersAsync(CancellationToken cancellationToken);
    }
}
