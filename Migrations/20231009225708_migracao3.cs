using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projeto_software_visual.Migrations
{
    public partial class migracao3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "IdEntregador",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "IdPetShop",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "IdPetShop",
                table: "Consultas");

            migrationBuilder.AddColumn<string>(
                name: "NomeCliente",
                table: "Entregas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeEntregador",
                table: "Entregas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomePetShop",
                table: "Entregas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeCliente",
                table: "Consultas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomePetShop",
                table: "Consultas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeCliente",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "NomeEntregador",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "NomePetShop",
                table: "Entregas");

            migrationBuilder.DropColumn(
                name: "NomeCliente",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "NomePetShop",
                table: "Consultas");

            migrationBuilder.AddColumn<int>(
                name: "IdCliente",
                table: "Entregas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEntregador",
                table: "Entregas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdPetShop",
                table: "Entregas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdCliente",
                table: "Consultas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdPetShop",
                table: "Consultas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
