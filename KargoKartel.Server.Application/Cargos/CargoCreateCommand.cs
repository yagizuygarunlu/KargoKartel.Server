using FluentValidation;
using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Common;
using KargoKartel.Server.Infrastructure.Contexts;
using Mapster;
using MediatR;

namespace KargoKartel.Server.Application.Cargos
{
    public sealed record CargoCreateCommand : IRequest<Result<string>>
    {
        public Person Sender { get; init; } = default!;
        public Person Receiver { get; init; } = default!;
        public Address ReceiveAddress { get; init; } = default!;
        public CargoInformation CargoInformation { get; init; } = default!;
    }
    public sealed record CargoInformationDto(
        CargoType CargoType, string CargoWeight);
    public sealed class CargoCreateCommandValidator : AbstractValidator<CargoCreateCommand>
    {
        public CargoCreateCommandValidator()
        {
            RuleFor(c => c.Sender).NotNull().WithMessage("Sender information is required.");
            RuleFor(c => c.Receiver).NotNull().WithMessage("Receiver information is required.");
            RuleFor(c => c.ReceiveAddress).NotNull().WithMessage("Receive address is required.");
            RuleFor(c => c.ReceiveAddress.City).NotNull().WithMessage("City is required.");
            RuleFor(c => c.ReceiveAddress.District).NotNull().WithMessage("District is required.");
            RuleFor(c => c.ReceiveAddress.Neighborhood).NotNull().WithMessage("Neighborhood is required.");
            RuleFor(c => c.CargoInformation.CargoType.Value)
                .GreaterThanOrEqualTo(0).WithMessage("Cargo type must be a valid value.")
                .LessThan(CargoType.List.Count).WithMessage("");
        }
    }

    internal sealed class CargoCreateCommandHandler(ICargoRepository cargoRepository, IUnitOfWork unitOfWork) : IRequestHandler<CargoCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CargoCreateCommand request, CancellationToken cancellationToken)
        {
            Cargo cargo = request.Adapt<Cargo>();

            CargoInformation cargoInformation = new CargoInformation(
                request.CargoInformation.CargoType,
                request.CargoInformation.CargoWeight);

            cargo.CargoInformation = cargoInformation;

            cargoRepository.Add(cargo);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Cargo created successfully";
        }
    }
}
