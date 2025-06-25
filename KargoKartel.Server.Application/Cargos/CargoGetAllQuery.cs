using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Common;
using MediatR;

namespace KargoKartel.Server.Application.Cargos
{
    public sealed record CargoGetAllQuery: IRequest<Result<List<CargoDto>>>;
    
    public sealed record CargoDto(
        int Id,
        Person Sender,
        Person Receiver,
        Address ReceiveAddress,
        CargoInformationDto CargoInformation);
}
