using MediatR;
using UDEM.DEVOPS.DogSitter.Application.Cuidador.Commands;
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
            .Produces(StatusCodes.Status201Created, typeof(CuidadorDto));

            //group.MapPut("/", async (IMediator mediator, UpdateCuidadorDto cuidadorDto) =>
            //{
            //    var updatedCuidador = await mediator.Send(new EditCuidadorCommand(cuidadorDto));
            //    return Results.Ok(updatedCuidador);
            //}).Produces(StatusCodes.Status200OK, typeof(CuidadorDto));

            //group.MapPatch("/", async (IMediator mediator, UpdateCuidadorDto cuidadorDto) =>
            //{
            //    var updatedCuidador = await mediator.Send(new PatchCuidadorCommand(cuidadorDto));
            //    return Results.Ok(updatedCuidador);
            //}).Produces(StatusCodes.Status200OK, typeof(CuidadorDto));

            //group.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
            //{
            //    await mediator.Send(new DeleteCuidadorCommand(id));
            //    return Results.Ok("Cuidador eliminado exitosamente");
            //});
            return group;
        }

    }

}
