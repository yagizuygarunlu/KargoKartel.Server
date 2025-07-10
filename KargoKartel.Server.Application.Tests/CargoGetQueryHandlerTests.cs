using System;
using System.Threading;
using System.Threading.Tasks;
using KargoKartel.Server.Application.Cargos;
using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Common;
using NSubstitute;
using Shouldly;
using Xunit;

public class CargoGetQueryHandlerTests
{
    private readonly ICargoRepository _cargoRepository = Substitute.For<ICargoRepository>();
    private readonly CargoGetQueryHandler _handler;

    public CargoGetQueryHandlerTests()
    {
        _handler = new CargoGetQueryHandler(_cargoRepository);
    }

    [Fact]
    public async Task Handle_Should_Return_Cargo_When_Found()
    {
        // Arrange
        var cargoId = Guid.NewGuid();
        var cargo = new Cargo();
        _cargoRepository.GetByIdAsync(cargoId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Cargo?>(cargo));
        var command = new CargoGetQuery(cargoId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldBe(cargo);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Cargo_Not_Found()
    {
        // Arrange
        var cargoId = Guid.NewGuid();
        _cargoRepository.GetByIdAsync(cargoId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Cargo?>(null));
        var command = new CargoGetQuery(cargoId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.ErrorMessages.ShouldContain("Cargo not found");
    }
} 