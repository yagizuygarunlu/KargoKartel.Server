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

public class CargoCreateCommandHandlerTests
{
    private readonly ICargoRepository _cargoRepository = Substitute.For<ICargoRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CargoCreateCommandHandler _handler;

    public CargoCreateCommandHandlerTests()
    {
        _handler = new CargoCreateCommandHandler(_cargoRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_Create_Cargo_And_Return_Success()
    {
        // Arrange
        var command = new CargoCreateCommand
        {
            Sender = new Person("Sender Name", "555-0000", "sender@mail.com", "11111111111"),
            Receiver = new Person("Receiver Name", "555-1111", "receiver@mail.com", "22222222222"),
            ReceiveAddress = new Address("TR", "Istanbul", "Kadikoy", "Moda", "Street", "1", "2", "34000", "Desc"),
            CargoInformation = new CargoInformation(CargoType.Package, 5)
        };
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldBe("Cargo created successfully");
     _cargoRepository.Received(1).Add(Arg.Any<Cargo>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
} 