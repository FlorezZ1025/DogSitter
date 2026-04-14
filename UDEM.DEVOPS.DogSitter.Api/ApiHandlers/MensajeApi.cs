namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers;

public static class MensajeApi
{
	public static RouteGroupBuilder MapMensaje(this IEndpointRouteBuilder routeHandler)
	{
		var group = routeHandler.MapGroup("/mensaje").WithTags("Mensaje");

		group.MapPost("/", async (string mensaje) =>
		{
			return Results.Ok($"Mensaje recibido: {mensaje}");
		}).Produces(StatusCodes.Status200OK, typeof(string));

		return group;
	}
}