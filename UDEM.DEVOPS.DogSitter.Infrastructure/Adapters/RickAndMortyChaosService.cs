using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Adapters
{
    public class RickAndMortyChaosService(HttpClient httpClient, ILogger<RickAndMortyChaosService> logger) : IRickAndMortyChaosService
    {
        private const string Endpoint = "https://rickandmortyapi.com/api/character/?page=19";
        public async Task<JsonNode?> GetCharactersAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("[ADAPTER-CHAOS] Sending request to Rick and Morty API...");

            // En Fase 1: sin try/catch — queremos ver el fallo crudo
            var response = await httpClient.GetAsync(Endpoint, cancellationToken);
            response.EnsureSuccessStatusCode(); // Lanza HttpRequestException si !2xx

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogInformation("[ADAPTER-CHAOS] Response received successfully.");

            return JsonNode.Parse(content);
        }
    }
}
