using KargoKartel.Server.Application.Cargos;
using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KargoKartel.Server.WebAPI.Endpoints
{
    public static class CargoEndpoints
    {
        public static void MapCargoEndpoints(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/api/v1").WithTags("Cargos")
            .RequireAuthorization();

            group.MapGet("/cargos", async ([FromServices] ISender sender, [AsParameters] CargoGetAllQuery request, CancellationToken cancellationToken) =>
            {
                var cargos = await sender.Send(request, cancellationToken);
                return Results.Ok(cargos);
            });

            group.MapGet("/cargos/{id}", async ([FromServices] ISender sender, [FromRoute] Guid id, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new CargoGetQuery(id), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.NotFound(response);
            })
                .Produces<Result<Cargo>>()
                .WithName("CargoGet");

            group.MapPost("/cargos", async ([FromServices] ISender sender, [FromBody] CargoCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
                .Produces<Result<string>>()
                .WithName("CargoCreate");

            group.MapPut("/cargos/{id}", async ([FromServices] ISender sender, [FromRoute] Guid id, [FromBody] CargoUpdateCommand request, CancellationToken cancellationToken) =>
            {
                if (id != request.CargoId)
                {
                    return Results.BadRequest("Cargo ID mismatch");
                }
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
                .Produces<Result<string>>()
                .WithName("CargoUpdate");

            group.MapPut("/cargos/{id}/status", async ([FromServices] ISender sender, [FromRoute] Guid id, [FromBody] CargoStatusUpdateCommand request, CancellationToken cancellationToken) => 
            {
                if (id != request.CargoId)
                {
                    return Results.BadRequest("Cargo ID mismatch");
                }
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
                .Produces<Result<string>>()
                .WithName("CargoStatusUpdate");

            group.MapDelete("/cargos/{id}", async ([FromServices] ISender sender, [FromRoute] Guid id, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new CargoDeleteCommand(id), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
                .Produces<Result<string>>()
                .WithName("CargoDelete");
        }
    }
}
