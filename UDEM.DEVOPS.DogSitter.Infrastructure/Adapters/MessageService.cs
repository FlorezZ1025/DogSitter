using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Adapters
{
    public class MessageService(HttpClient httpClient, ILogger<MessageService> logger) : IMessageService
    {
        private readonly string _yecidUrl = "https://cart-api-orders-88266388657.us-central1.run.app/api/v2";

        public async Task<JsonNode> EnviarMensaje(CuidadorDto cuidador)
        {
            var url = $"{_yecidUrl}/mensaje";
            var payload = new
            {
                id = cuidador.Id,
                nombre = cuidador.nombre,
                telefono = cuidador.telefono,
                email = cuidador.email,
                fechaInicioExperiencia = cuidador.fechaInicioExperiencia,
                direccion = cuidador.direccion,
                activo = cuidador.activo
            };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(jsonPayload , Encoding.UTF8, "application/json")
            };

            logger.LogInformation("Enviando mensaje a la API de Yecid para el cuidador: {Nombre}", cuidador.nombre);
            logger.LogInformation(jsonPayload);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonNode.Parse(json) ?? new JsonObject();
        }

        public async Task<JsonNode> ProbarApiYecid()
        {
            var url = $"{_yecidUrl}/users";
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonNode.Parse(json) ?? new JsonObject();
        }
    }
}
