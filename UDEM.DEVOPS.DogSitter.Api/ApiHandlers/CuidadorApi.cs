using UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands;
using UDEM.DEVOPS.DogSitter.Application.Cuidador.Queries;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using MediatR;

namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers
{
    public static class CuidadorApi
    {
        public static RouteGroupBuilder MapCuidador(this IEndpointRouteBuilder routeHandler)
        {
            var group = routeHandler.MapGroup("/api/cuidador").WithTags("Cuidador");
            group.MapGet("/{id}", async (IMediator mediator, Guid id) =>
            {
                return Results.Ok(await mediator.Send(new GetCuidadorQuery(id)));
            })
            .Produces(StatusCodes.Status200OK, typeof(CuidadorDto));

            group.MapPost("/", async (IMediator mediator, CuidadorDto cuidadorDto) =>
            {
                var createdCuidador = await mediator.Send(new RegisterCuidadorCommand(cuidadorDto));
                return Results.Created($"/{Guid.NewGuid()}", cuidadorDto);
            })
            .Produces(StatusCodes.Status201Created, typeof(CuidadorDto));
            return group;
        }
    }
}
