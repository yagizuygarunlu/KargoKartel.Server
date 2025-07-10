using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KargoKartel.Server.Application.Cargos;
using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Common;
using KargoKartel.Server.Domain.Users;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Shouldly;
using Xunit;

public class CargoGetAllQueryHandlerTests
{
    private readonly ICargoRepository _cargoRepository = Substitute.For<ICargoRepository>();
    private readonly UserManager<AppUser> _userManager;
    private readonly CargoGetAllQueryHandler _handler;

    public CargoGetAllQueryHandlerTests()
    {
        var userStore = Substitute.For<IUserStore<AppUser>>();
        _userManager = Substitute.For<UserManager<AppUser>>(userStore, null, null, null, null, null, null, null, null);
        _handler = new CargoGetAllQueryHandler(_cargoRepository, _userManager);
    }

    [Fact]
    public async Task Handle_Should_Return_CargoDtos()
    {
        // Arrange
        var cargos = new List<Cargo>
        {
            new Cargo
            {
                Sender = new Person("Sender", "555-0000", "sender@mail.com", "11111111111"),
                Receiver = new Person("Receiver", "555-1111", "receiver@mail.com", "22222222222"),
                ReceiveAddress = new Address("TR", "Istanbul", "Kadikoy", "Moda", "Street", "1", "2", "34000", "Desc"),
                CargoInformation = new CargoInformation(CargoType.Package, 5),
                Status = Status.Pending,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = Guid.NewGuid(),
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = Guid.NewGuid()
            }
        };
        var createUser = new AppUser { Id = cargos[0].CreatedBy, FirstName = "Create", LastName = "User" };
        var updateUser = new AppUser { Id = cargos[0].UpdatedBy.Value, FirstName = "Update", LastName = "User" };
        _cargoRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(callInfo => Task.FromResult(cargos.ToList()));
        _userManager.Users.Returns(new List<AppUser> { createUser, updateUser }.AsQueryable());

        // Act
        var result = await _handler.Handle(new CargoGetAllQuery(), CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Count.ShouldBe(1);
        result.Data[0].SenderFullName.ShouldBe("Sender");
        result.Data[0].CreateUserFullName.ShouldBe("Create User");
        result.Data[0].UpdateUserFullName.ShouldBe("Update User");
    }
} 