using MediatR;
using DGW.DogSitter.DogWalker.Api.Filters;
using DGW.DogSitter.DogWalker.Application.Voters;
using DGW.DogSitter.DogWalker.Domain.Dtos;

namespace DGW.DogSitter.DogWalker.Api.ApiHandlers;

public static class VoterApi
{
    public static RouteGroupBuilder MapVoter(this IEndpointRouteBuilder routeHandler)
    {
        routeHandler.MapGet("/{id}", async (IMediator mediator, Guid id) =>
        {
            return Results.Ok(await mediator.Send(new VoterQuery(id)));
        })
        .Produces(StatusCodes.Status200OK, typeof(VoterDto));

        routeHandler.MapGet("/dapper/{id}", async (IMediator mediator, Guid id) =>
        {
            return Results.Ok(await mediator.Send(new VoterSimpleQuery(id)));
        })
        .Produces(StatusCodes.Status200OK, typeof(VoterDto));

        routeHandler.MapPost("/", async (IMediator mediator, [Validate] VoterRegisterCommand voter) =>
        {
            var vote = await mediator.Send(voter);
            return Results.Created(new Uri($"/api/voter/{vote}", UriKind.Relative), vote);
        })
        .Produces(statusCode: StatusCodes.Status201Created);

        return (RouteGroupBuilder)routeHandler;
    }
}