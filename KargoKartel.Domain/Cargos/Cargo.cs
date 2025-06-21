using Ardalis.SmartEnum;
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
    public sealed record Person(
        string Name, string Phone, string Email, string IdentityNumber);

    public sealed record Address(
        string Country, string City, string District, string Neighborhood,
        string Street, string BuildingNumber, string ApartmentNumber,
        string PostalCode, string Description)
    {
        public override string ToString()
        {
            return $"{Country}, {City}, {District}, {Neighborhood}, {Street} " +
                   $"{BuildingNumber}/{ApartmentNumber}, {PostalCode}, {Description}";
        }
    }

    public sealed record CargoInformation(
        string CargoType, string CargoWeight);

    public sealed class CargoType : SmartEnum<CargoType>
    {
        public static CargoType Package = new("Package", 0);
        public static CargoType Pallet = new("Pallet", 1);
        public static CargoType Container = new("Container", 2);
        public static CargoType Document = new("Document", 3);
        public static CargoType Other = new("Other", 4);

        public CargoType(string name, int value) : base(name, value) { }
    }
}
