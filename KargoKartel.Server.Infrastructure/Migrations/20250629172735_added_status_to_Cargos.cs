using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KargoKartel.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_status_to_Cargos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CargoInformation_CargoWeight",
                table: "Cargos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Cargos",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cargos");

            migrationBuilder.AlterColumn<string>(
                name: "CargoInformation_CargoWeight",
                table: "Cargos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
