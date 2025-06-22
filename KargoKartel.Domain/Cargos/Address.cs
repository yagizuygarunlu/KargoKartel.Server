namespace KargoKartel.Server.Domain.Cargos
{
    #region

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
    #endregion
}

