using FluentValidation;
using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Common;
using KargoKartel.Server.Infrastructure.Contexts;
using MediatR;

namespace KargoKartel.Server.Application.Cargos
{
    public sealed record CargoUpdateCommand: IRequest<Result<string>>
    {
        public CargoUpdateCommand(Guid cargoId, Person sender, Person receiver, Address receiveAddress, CargoInformation cargoInformation)
        {
            CargoId = cargoId;
            Sender = sender;
            Receiver = receiver;
            ReceiveAddress = receiveAddress;
            CargoInformation = cargoInformation;
        }
        public Guid CargoId { get; init; }
        public Person Sender { get; init; } = default!;
        public Person Receiver { get; init; } = default!;
        public Address ReceiveAddress { get; init; } = default!;
        public CargoInformation CargoInformation { get; init; } = default!;
    }

    internal sealed class CargoUpdateCommandValidator : AbstractValidator<CargoUpdateCommand>
    {
        public CargoUpdateCommandValidator()
        {
            RuleFor(c => c.CargoId).NotEmpty().WithMessage("Cargo ID is required.");
            RuleFor(c => c.Sender).NotNull().WithMessage("Sender information is required.");
            RuleFor(c => c.Receiver).NotNull().WithMessage("Receiver information is required.");
            RuleFor(c => c.ReceiveAddress).NotNull().WithMessage("Receive address is required.");
            RuleFor(c => c.ReceiveAddress.City).NotNull().WithMessage("City is required.");
            RuleFor(c => c.ReceiveAddress.District).NotNull().WithMessage("District is required.");
            RuleFor(c => c.ReceiveAddress.Neighborhood).NotNull().WithMessage("Neighborhood is required.");
            RuleFor(c => c.CargoInformation.CargoType.Value)
                .GreaterThanOrEqualTo(0).WithMessage("Cargo type must be a valid value.")
                .LessThan(CargoType.List.Count);
        }
    }
    internal sealed class CargoUpdateCommandHandler(ICargoRepository cargoRepository, IUnitOfWork unitOfWork) : IRequestHandler<CargoUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CargoUpdateCommand request, CancellationToken cancellationToken)
        {
            Cargo? cargo = await cargoRepository.GetByIdAsync(request.CargoId, cancellationToken);
            if (cargo is null)
                return Result<string>.Failure("Cargo not found");

            if (cargo.Status != Status.Pending)
                return Result<string>.Failure("Cargo can only be updated when it is in pending status");

            cargo.Sender = request.Sender;
            cargo.Receiver = request.Receiver;
            cargo.ReceiveAddress = request.ReceiveAddress;
            cargo.CargoInformation = request.CargoInformation;
            cargoRepository.Update(cargo);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<string>.Succeed("Cargo updated successfully");
        }
    }
}
