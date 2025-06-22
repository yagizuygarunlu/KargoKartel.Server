using Ardalis.SmartEnum;

namespace KargoKartel.Server.Domain.Cargos
{
    #region

    public sealed class CargoType : SmartEnum<CargoType>
    {
        public static CargoType Package = new("Package", 0);
        public static CargoType Pallet = new("Pallet", 1);
        public static CargoType Container = new("Container", 2);
        public static CargoType Document = new("Document", 3);
        public static CargoType Other = new("Other", 4);

        public CargoType(string name, int value) : base(name, value) { }
    }
    #endregion
}

