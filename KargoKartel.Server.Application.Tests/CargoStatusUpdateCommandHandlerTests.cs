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

public class CargoStatusUpdateCommandHandlerTests
{
    private readonly ICargoRepository _cargoRepository = Substitute.For<ICargoRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CargoStatusUpdateCommandHandler _handler;

    public CargoStatusUpdateCommandHandlerTests()
    {
        _handler = new CargoStatusUpdateCommandHandler(_cargoRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_Update_Status_When_Cargo_Exists()
    {
        // Arrange
        var cargoId = Guid.NewGuid();
        var cargo = new Cargo { Status = Status.Pending };
        _cargoRepository.GetByIdAsync(cargoId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Cargo?>(cargo));
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));
        var command = new CargoStatusUpdateCommand { CargoId = cargoId, StatusValue = Status.Delivered.Value };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldBe("Cargo status updated successfully.");
        cargo.Status.Value.ShouldBe(Status.Delivered.Value);
        _cargoRepository.Received(1).Update(cargo);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Cargo_Not_Found()
    {
        // Arrange
        var cargoId = Guid.NewGuid();
        _cargoRepository.GetByIdAsync(cargoId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Cargo?>(null));
        var command = new CargoStatusUpdateCommand { CargoId = cargoId, StatusValue = Status.Delivered.Value };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.ErrorMessages.ShouldContain("Cargo not found.");
    }
} 