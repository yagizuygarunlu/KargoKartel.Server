using System;
using System.Threading;
using System.Threading.Tasks;
using KargoKartel.Server.Application.Cargos;
using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Common;
using KargoKartel.Server.Infrastructure.Contexts;
using NSubstitute;
using Shouldly;
using Xunit;

public class CargoUpdateCommandHandlerTests
{
    private readonly ICargoRepository _cargoRepository = Substitute.For<ICargoRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CargoUpdateCommandHandler _handler;

    public CargoUpdateCommandHandlerTests()
    {
        _handler = new CargoUpdateCommandHandler(_cargoRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_Update_Cargo_When_Pending()
    {
        // Arrange
        var cargoId = Guid.NewGuid();
        var cargo = new Cargo { Status = Status.Pending };
        _cargoRepository.GetByIdAsync(cargoId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Cargo?>(cargo));
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));
        var command = new CargoUpdateCommand(cargoId,
            new Person("Sender", "555-0000", "sender@mail.com", "11111111111"),
            new Person("Receiver", "555-1111", "receiver@mail.com", "22222222222"),
            new Address("TR", "Istanbul", "Kadikoy", "Moda", "Street", "1", "2", "34000", "Desc"),
            new CargoInformation(CargoType.Package, 5));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldBe("Cargo updated successfully");
        _cargoRepository.Received(1).Update(Arg.Any<Cargo>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Cargo_Not_Found()
    {
        // Arrange
        var cargoId = Guid.NewGuid();
        _cargoRepository.GetByIdAsync(cargoId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Cargo?>(null));
        var command = new CargoUpdateCommand(cargoId,
            new Person("Sender", "555-0000", "sender@mail.com", "11111111111"),
            new Person("Receiver", "555-1111", "receiver@mail.com", "22222222222"),
            new Address("TR", "Istanbul", "Kadikoy", "Moda", "Street", "1", "2", "34000", "Desc"),
            new CargoInformation(CargoType.Package, 5));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.ErrorMessages.ShouldContain("Cargo not found");
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Cargo_Status_Not_Pending()
    {
        // Arrange
        var cargoId = Guid.NewGuid();
        var cargo = new Cargo { Status = Status.Delivered };
        _cargoRepository.GetByIdAsync(cargoId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Cargo?>(cargo));
        var command = new CargoUpdateCommand(cargoId,
            new Person("Sender", "555-0000", "sender@mail.com", "11111111111"),
            new Person("Receiver", "555-1111", "receiver@mail.com", "22222222222"),
            new Address("TR", "Istanbul", "Kadikoy", "Moda", "Street", "1", "2", "34000", "Desc"),
            new CargoInformation(CargoType.Package, 5));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.ErrorMessages.ShouldContain("Cargo can only be updated when it is in pending status");
    }
} 