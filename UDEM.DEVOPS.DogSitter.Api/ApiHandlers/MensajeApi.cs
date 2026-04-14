using System.Text.Json.Nodes;
using MediatR;
using UDEM.DEVOPS.DogSitter.Application.Mensaje.Queries;
namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers;

public static class MensajeApi
{
	public static RouteGroupBuilder MapMensaje(this IEndpointRouteBuilder routeHandler)
	{
		var group = routeHandler.MapGroup("/mensaje").WithTags("Mensaje");

		group.MapPost("/", async () =>
		{
			return Results.Ok($"Aquí me conectaré con las APIs de mis compañerosss");
		}).Produces(StatusCodes.Status200OK, typeof(string));

		group.MapGet("/", async (IMediator mediator) =>
		{
			var result = await mediator.Send(new GetUsuariosFromYecidQuery());
			return Results.Ok(result);
		}).Produces(StatusCodes.Status200OK, typeof(JsonNode));
		return group;
	}
}