using MediatR;
using UDEM.DEVOPS.DogSitter.Application.Raza.Commands;
using UDEM.DEVOPS.DogSitter.Application.Raza.Queries;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers
{
    public static class RazaApi
    {
        public static RouteGroupBuilder MapRaza(this IEndpointRouteBuilder routeHandler)
        {
            var group = routeHandler.MapGroup("/api/raza").WithTags("Raza");

            group.MapGet("/", async (IMediator mediator) =>
            {
                var razas = await mediator.Send(new GetAllRazasQuery());
                return Results.Ok(razas);
            });

            group.MapGet("/{id}", async (IMediator mediator, Guid id) =>
            {
                return Results.Ok(await mediator.Send(new GetRazaQuery(id)));
            })
            .Produces(StatusCodes.Status200OK, typeof(RazaDto));

            group.MapPost("/", async (IMediator mediator, CreateRazaDto razaDto) =>
            {
                var createdRaza = await mediator.Send(new RegisterRazaCommand(razaDto));

                return Results.Created(new Uri($"/api/raza/{createdRaza.Id}", UriKind.Relative), createdRaza);
            })
            .Produces(StatusCodes.Status201Created, typeof(RazaDto));

            group.MapPut("/", async (IMediator mediator, UpdateRazaDto razaDto) =>
            {
                var updatedRaza = await mediator.Send(new EditRazaCommand(razaDto));
                return Results.Ok(updatedRaza);
            }).Produces(StatusCodes.Status200OK, typeof(RazaDto));

            group.MapPatch("/", async (IMediator mediator, UpdateRazaDto razaDto) =>
            {
                var updatedRaza = await mediator.Send(new PatchRazaCommand(razaDto));
                return Results.Ok(updatedRaza);
            }).Produces(StatusCodes.Status200OK, typeof(RazaDto));

            group.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
            {
                await mediator.Send(new DeleteRazaCommand(id));
                return Results.Ok("Raza eliminada exitosamente");
            });
            return group;
        }

    }

}
