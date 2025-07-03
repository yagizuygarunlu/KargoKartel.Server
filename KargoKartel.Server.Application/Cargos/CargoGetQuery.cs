using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Common;
using MediatR;

namespace KargoKartel.Server.Application.Cargos
{
    public sealed record CargoGetQuery : IRequest<Result<Cargo>>
    {
        public CargoGetQuery(Guid cargoId)
        {
            CargoId = cargoId;
        }
        public Guid CargoId { get; init; }
    }
    internal sealed class CargoGetQueryHandler(ICargoRepository cargoRepository) : IRequestHandler<CargoGetQuery, Result<Cargo>>
    {
        public async Task<Result<Cargo>> Handle(CargoGetQuery request, CancellationToken cancellationToken)
        {
            Cargo? cargo = await cargoRepository.GetByIdAsync(request.CargoId, cancellationToken);
            if (cargo is null)
                return Result<Cargo>.Failure("Cargo not found");
            
            return Result<Cargo>.Succeed(cargo);
        }
    }
}

