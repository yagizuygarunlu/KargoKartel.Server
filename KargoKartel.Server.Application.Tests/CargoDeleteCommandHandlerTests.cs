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

public class CargoDeleteCommandHandlerTests
{
    private readonly ICargoRepository _cargoRepository = Substitute.For<ICargoRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CargoDeleteCommandHandler _handler;

    public CargoDeleteCommandHandlerTests()
    {
        _handler = new CargoDeleteCommandHandler(_cargoRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_Delete_Cargo_When_Pending()
    {
        // Arrange
        var cargoId = Guid.NewGuid();
        var cargo = new Cargo { Status = Status.Pending };
        _cargoRepository.GetByIdAsync(cargoId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Cargo?>(cargo));
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));
        var command = new CargoDeleteCommand(cargoId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldBe("Cargo deleted successfully");
        _cargoRepository.Received(1).Update(Arg.Is<Cargo>(c => c.IsDeleted));
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Cargo_Not_Found()
    {
        // Arrange
        var cargoId = Guid.NewGuid();
        _cargoRepository.GetByIdAsync(cargoId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Cargo?>(null));
        var command = new CargoDeleteCommand(cargoId);

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
        var command = new CargoDeleteCommand(cargoId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.ErrorMessages.ShouldContain("Cargo can only be deleted when it is in pending status");
    }
} 