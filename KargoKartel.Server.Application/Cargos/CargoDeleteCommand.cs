using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Common;
using KargoKartel.Server.Infrastructure.Contexts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KargoKartel.Server.Application.Cargos
{
    public sealed record CargoDeleteCommand : IRequest<Result<string>>
    {
        public CargoDeleteCommand(Guid cargoId)
        {
            CargoId = cargoId;
        }
        public Guid CargoId { get; init; }
    }
    internal sealed class CargoDeleteCommandHandler(ICargoRepository _cargoRepository, IUnitOfWork _unitOfWork) : IRequestHandler<CargoDeleteCommand, Result<string>>
    {

        public async Task<Result<string>> Handle(CargoDeleteCommand request, CancellationToken cancellationToken)
        {
            Cargo? cargo = await _cargoRepository.GetByIdAsync(request.CargoId, cancellationToken);
            if (cargo is null)
                return Domain.Common.Result<string>.Failure("Cargo not found");
            
            if (cargo.Status != Status.Pending)
                return Domain.Common.Result<string>.Failure("Cargo can only be deleted when it is in pending status");
            
            cargo.IsDeleted = true;
            _cargoRepository.Update(cargo);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<string>.Succeed("Cargo deleted successfully");
        }
    }
}
