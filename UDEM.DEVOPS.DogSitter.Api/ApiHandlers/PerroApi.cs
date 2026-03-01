using MediatR;
using UDEM.DEVOPS.DogSitter.Application.Perro.Commands;
using UDEM.DEVOPS.DogSitter.Application.Perro.Queries;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers
{
    public static class PerroApi
    {
            public static RouteGroupBuilder MapPerro(this IEndpointRouteBuilder routeHandler)
            {
                var group = routeHandler.MapGroup("/api/perro").WithTags("Perro");

            group.MapGet("/", async (IMediator mediator) =>
            {
                var perros = await mediator.Send(new GetAllPerrosQuery());
                return Results.Ok(perros);
            });

            group.MapGet("/{id}", async (IMediator mediator, Guid id) =>
            {
                return Results.Ok(await mediator.Send(new GetPerroQuery(id)));
            })
            .Produces(StatusCodes.Status200OK, typeof(PerroDto));

            group.MapPost("/", async (IMediator mediator, CreatePerroDto perroDto) =>
            {
                var createdPerro = await mediator.Send(new RegisterPerroCommand(perroDto));

                return Results.Created(new Uri($"/api/perro/{createdPerro.Id}", UriKind.Relative), createdPerro);
            })
            .Produces(StatusCodes.Status201Created, typeof(PerroDto));

            group.MapPut("/", async (IMediator mediator, UpdatePerroDto perroDto) =>
            {
                var updatedPerro = await mediator.Send(new EditPerroCommand(perroDto));
                return Results.Ok(updatedPerro);
            }).Produces(StatusCodes.Status200OK, typeof(PerroDto));

            group.MapPatch("/", async (IMediator mediator, UpdatePerroDto perroDto) =>
            {
                var updatedPerro = await mediator.Send(new PatchPerroCommand(perroDto));
                return Results.Ok(updatedPerro);
            }).Produces(StatusCodes.Status200OK, typeof(PerroDto));

            //        group.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
            //        {
            //            await mediator.Send(new DeletePerroCommand(id));
            //            return Results.Ok("Perro eliminado exitosamente");
            //        });
            return group;
        }
    }
}
