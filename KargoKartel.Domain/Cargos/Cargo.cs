using KargoKartel.Server.Domain.Abstractions;

namespace KargoKartel.Server.Domain.Cargos
{
    public sealed class Cargo : Entity
    {
        public Person Sender { get; set; } = default!;
        public Person Receiver { get; set; } = default!;
        public Address ReceiveAddress { get; set; } = default!;
        public CargoInformation CargoInformation { get; set; } = default!;
    }
}

