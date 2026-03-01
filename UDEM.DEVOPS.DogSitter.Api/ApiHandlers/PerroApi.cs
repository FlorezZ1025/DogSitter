namespace UDEM.DEVOPS.DogSitter.Api.ApiHandlers
{
    public static class PerroApi
    {
            public static RouteGroupBuilder MapPerro(this IEndpointRouteBuilder routeHandler)
            {
                var group = routeHandler.MapGroup("/api/perro").WithTags("Perro");

            //        group.MapGet("/", async (IMediator mediator) =>
            //        {
            //            var perros = await mediator.Send(new Application.Perro.Queries.GetAllPerrosQuery());
            //            return Results.Ok(perros);
            //        });

            //        group.MapGet("/{id}", async (IMediator mediator, Guid id) =>
            //        {
            //            return Results.Ok(await mediator.Send(new Application.Perro.Queries.GetPerroQuery(id)));
            //        })
            //        .Produces(StatusCodes.Status200OK, typeof(Domain.Dtos.PerroDto));

            //        group.MapPost("/", async (IMediator mediator, Domain.Dtos.CreatePerroDto perroDto) =>
            //        {
            //            var createdPerro = await mediator.Send(new Application.Perro.Commands.RegisterPerroCommand(perroDto));

            //            return Results.Created(new Uri($"/api/perro/{createdPerro.Id}", UriKind.Relative), createdPerro);
            //        })
            //        .Produces(StatusCodes.Status201Created, typeof(Domain.Dtos.PerroDto));

            //        group.MapPut("/", async (IMediator mediator, Domain.Dtos.UpdatePerroDto perroDto) =>
            //        {
            //            var updatedPerro = await mediator.Send(new Application.Perro.Commands.EditPerroCommand(perroDto));
            //            return Results.Ok(updatedPerro);
            //        }).Produces(StatusCodes.Status200OK, typeof(Domain.Dtos.PerroDto));

            //        group.MapPatch("/", async (IMediator mediator, Domain.Dtos.UpdatePerroDto perroDto) =>
            //        {
            //            var updatedPerro = await mediator.Send(new Application.Perro.Commands.PatchPerroCommand(perroDto));
            //            return Results.Ok(updatedPerro);
            //        }).Produces(StatusCodes.Status200OK, typeof(Domain.Dtos.PerroDto));

            //        group.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
            //        {
            //            await mediator.Send(new Application.Perro.Commands.DeletePerroCommand(id));
            //            return Results.Ok("Perro eliminado exitosamente");
            //        });
            return group;
        }
    }
}
