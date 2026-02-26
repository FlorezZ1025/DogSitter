using UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands;
using UDEM.DEVOPS.DogSitter.Application.Cuidador.Queries;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using MediatR;
using UDEM.DEVOPS.DogSitter.Api.Filters;

namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers
{
    public static class CuidadorApi
    {
        public static RouteGroupBuilder MapCuidador(this IEndpointRouteBuilder routeHandler)
        {
            var group = routeHandler.MapGroup("/api/cuidador").WithTags("Cuidador");

            group.MapGet("/", async (IMediator mediator) =>
            {
                var cuidadores = await mediator.Send(new GetAllCuidadoresQuery());
                return Results.Ok(cuidadores);
            });

            group.MapGet("/{id}", async (IMediator mediator, Guid id) =>
            {
                return Results.Ok(await mediator.Send(new GetCuidadorQuery(id)));
            })
            .Produces(StatusCodes.Status200OK, typeof(CuidadorDto));

            group.MapPost("/", async (IMediator mediator, CreateCuidadorDto cuidadorDto) =>
            {
                var createdCuidador = await mediator.Send(new RegisterCuidadorCommand(cuidadorDto));

                return Results.Created(new Uri($"/api/cuidador/{createdCuidador.Id}", UriKind.Relative), createdCuidador);
            })
            .Produces(StatusCodes.Status201Created, typeof(CuidadorDto));

            group.MapPut("/", async (IMediator mediator, UpdateCuidadorDto cuidadorDto) =>
            {
                var updatedCuidador = await mediator.Send(new EditCuidadorCommand(cuidadorDto));
                return Results.Ok(updatedCuidador);
            }).Produces(StatusCodes.Status200OK, typeof(CuidadorDto));

            group.MapPatch("/", async (IMediator mediator, UpdateCuidadorDto cuidadorDto) =>
            {
                var updatedCuidador = await mediator.Send(new PatchCuidadorCommand(cuidadorDto));
                return Results.Ok(updatedCuidador);
            }).Produces(StatusCodes.Status200OK, typeof(CuidadorDto));

            group.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
            {
                await mediator.Send(new DeleteCuidadorCommand(id));
                return Results.Ok("Cuidador eliminado exitosamente");
            });
            return group;
        }
    }
}
