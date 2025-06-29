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
            RouteGroupBuilder group = app.MapGroup("/api/v1").WithTags("Cargos");
            //.RequireAuthorization();

            group.MapGet("/cargos", async ([FromServices] ISender sender, [AsParameters] CargoGetAllQuery request, CancellationToken cancellationToken) =>
            {
                var cargos = await sender.Send(request, cancellationToken);
                return Results.Ok(cargos);
            });
            //group.MapGet("/cargos/{id}", async (ICargoRepository cargoRepository, int id) =>
            //{
            //    var cargo = await cargoRepository.GetByIdAsync(id);
            //    return cargo is not null ? Results.Ok(cargo) : Results.NotFound();
            //});

            group.MapPost("/cargos", async ([FromServices] ISender sender, [FromBody] CargoCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
                .Produces<Result<string>>()
                .WithName("CargoCreate");

            //group.MapPut("/cargos/{id}", async (ICargoRepository cargoRepository, int id, Cargo updatedCargo, IUnitOfWork unitOfWork) =>
            //{
            //    if (id != updatedCargo.Id)
            //    {
            //        return Results.BadRequest("Cargo ID mismatch.");
            //    }
            //    var existingCargo = await cargoRepository.GetByIdAsync(id);
            //    if (existingCargo is null)
            //    {
            //        return Results.NotFound();
            //    }
            //    await cargoRepository.UpdateAsync(updatedCargo);
            //    await unitOfWork.SaveChangesAsync();
            //    return Results.NoContent();
            //});
            //group.MapDelete("/cargos/{id}", async (ICargoRepository cargoRepository, int id, IUnitOfWork unitOfWork) =>
            //{
            //    var existingCargo = await cargoRepository.GetByIdAsync(id);
            //    if (existingCargo is null)
            //    {
            //        return Results.NotFound();
            //    }
            //    await cargoRepository.DeleteAsync(existingCargo);
            //    await unitOfWork.SaveChangesAsync();
            //    return Results.NoContent();
            //});
        }
    }
}
