using System.Text.Json.Nodes;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers;

public static class RickAndMortyApi
{
public static RouteGroupBuilder MapRickAndMorty(this IEndpointRouteBuilder routeHandler)
    {
        var group = routeHandler.MapGroup("/rickandmorty").WithTags("Rick and Morty");
        ILogger<RouteGroupBuilder> _logger = new Logger<RouteGroupBuilder>(new LoggerFactory());

        group.MapGet("with-retries", async (IRickAndMortyService rickAndMortyService) =>
        {
            _logger.LogInformation("[CONTROLLER] GET /api/rickandmorty/with-retries invoked.");

            try
            {
                var result = await rickAndMortyService.GetCharactersAsync(CancellationToken.None);

                return Results.Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex,
                    "[CONTROLLER] All retries exhausted. Returning degraded response.");

                return Results.Json(new
                {
                    error = "character_service_unavailable",
                    message = "The character service is temporarily unavailable. Please try again later.",
                    retried = true
                }, statusCode: StatusCodes.Status503ServiceUnavailable);
            }
        }).Produces(StatusCodes.Status200OK, typeof(JsonNode));

        group.MapGet("chaos-always", async (IRickAndMortyChaosService rickAndMortyService) =>
        {
            _logger.LogInformation("[CONTROLLER] GET /api/rickandmorty/chaos-always invoked.");

                var result = await rickAndMortyService.GetCharactersAsync(CancellationToken.None);

                return Results.Ok(result);
 
        }).Produces(StatusCodes.Status200OK, typeof(JsonNode));

        return group;
    }
}

