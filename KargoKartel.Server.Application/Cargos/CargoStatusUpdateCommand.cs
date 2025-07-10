using FluentValidation;
using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Common;
using KargoKartel.Server.Infrastructure.Contexts;
using MediatR;

namespace KargoKartel.Server.Application.Cargos
{
    public sealed record CargoStatusUpdateCommand : IRequest<Result<string>>
    {
        public Guid CargoId { get; init; }
        public int StatusValue { get; init; }

    }
    public sealed class CargoStatusUpdateCommandValidator : AbstractValidator<CargoStatusUpdateCommand>
    {
        public CargoStatusUpdateCommandValidator()
        {
            RuleFor(x => x.CargoId).NotEmpty().WithMessage("Cargo ID is required.");
            RuleFor(x => x.StatusValue).IsInEnum().WithMessage("Invalid status value.");
        }
    }
    internal sealed class CargoStatusUpdateCommandHandler(ICargoRepository cargoRepository, IUnitOfWork unitOfWork) : IRequestHandler<CargoStatusUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CargoStatusUpdateCommand request, CancellationToken cancellationToken)
        {
            var cargo = await cargoRepository.GetByIdAsync(request.CargoId, cancellationToken);
            if (cargo is null)
                return Result<string>.Failure("Cargo not found.");

            cargo.Status = (Status)request.StatusValue;
            cargoRepository.Update(cargo);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return "Cargo status updated successfully.";
        }
    }
}
