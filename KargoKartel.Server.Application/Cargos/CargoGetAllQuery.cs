using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Common;
using KargoKartel.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace KargoKartel.Server.Application.Cargos
{
    public sealed record CargoGetAllQuery : IRequest<Result<List<CargoDto>>>;

    public sealed record CargoDto(
      string SenderFullName,
      string ReceiverFullName,
      string ReceiverCity,
      string ReceiverDistrict,
      string CargoType,
      int Weight,
      string Status,
      DateTimeOffset CreatedAt,
      string CreateUserFullName,
        DateTimeOffset? UpdatedAt,
      string UpdateUserFullName);

    internal sealed class CargoGetAllQueryHandler : IRequestHandler<CargoGetAllQuery, Result<List<CargoDto>>>
    {
        private readonly ICargoRepository _cargoRepository;
        private readonly UserManager<AppUser> _userManager;

        public CargoGetAllQueryHandler(ICargoRepository cargoRepository, UserManager<AppUser> userManager)
        {
            _cargoRepository = cargoRepository;
            _userManager = userManager;
        }

        public async Task<Result<List<CargoDto>>> Handle(CargoGetAllQuery request, CancellationToken cancellationToken)
        {
            var cargos = await _cargoRepository.GetAllAsync(cancellationToken);
            var cargoDtos =  (from cargo in cargos
                             join create_user in _userManager.Users.AsQueryable() on cargo.CreatedBy equals create_user.Id
                             join update_user in _userManager.Users.AsQueryable() on cargo.UpdatedBy equals update_user.Id into update_user
                             from update_users in update_user.DefaultIfEmpty()
                             select new CargoDto(
                                    SenderFullName: cargo.Sender.Name,
                                    ReceiverFullName: cargo.Receiver.Name,
                                    ReceiverCity: cargo.ReceiveAddress.City,
                                    ReceiverDistrict: cargo.ReceiveAddress.District,
                                    CargoType: cargo.CargoInformation.CargoType.Name,
                                    Weight: cargo.CargoInformation.CargoWeight,
                                    Status: cargo.Status.Name,
                                    CreatedAt: cargo.CreatedAt,
                                    CreateUserFullName: create_user.FullName,
                                    UpdatedAt: cargo.UpdatedAt,
                                    UpdateUserFullName: update_users?.FullName ?? "N/A"
                              )).ToList();

            return Result<List<CargoDto>>.Succeed(cargoDtos);
        }
    }
}
