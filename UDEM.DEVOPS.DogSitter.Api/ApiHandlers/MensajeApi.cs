using System.Text.Json.Nodes;
using MediatR;
using UDEM.DEVOPS.DogSitter.Application.Mensaje.Commands;
using UDEM.DEVOPS.DogSitter.Application.Mensaje.Queries;
namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers;

public static class MensajeApi
{
	public static RouteGroupBuilder MapMensaje(this IEndpointRouteBuilder routeHandler)
	{
		var group = routeHandler.MapGroup("/mensaje").WithTags("Mensaje");

		group.MapPost("/", async (IMediator mediator) =>
		{
			var result = await mediator.Send(new SendMensajeToYecidCommand());
            return Results.Ok(result);
		}).Produces(StatusCodes.Status200OK, typeof(string));

		group.MapGet("/", async (IMediator mediator) =>
		{
			var result = await mediator.Send(new GetUsuariosFromYecidQuery());
			return Results.Ok(result);
		}).Produces(StatusCodes.Status200OK, typeof(JsonNode));
		return group;
	}
}