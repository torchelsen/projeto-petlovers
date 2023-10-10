using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projeto_software_visual.Migrations
{
    public partial class migracao2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nome",
                table: "PetShops",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "PetShops",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PetShops",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "Entregadores",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Entregadores",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Entregadores",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Clientes",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "Clientes",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Clientes",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Clientes",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "PetShops",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "PetShops",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Entregadores",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Entregadores",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: false),
                    IdPetShop = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entregas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCliente = table.Column<int>(type: "INTEGER", nullable: false),
                    IdEntregador = table.Column<int>(type: "INTEGER", nullable: false),
                    IdPetShop = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entregas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "Entregas");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "PetShops",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "PetShops",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PetShops",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Entregadores",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Entregadores",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Entregadores",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Clientes",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Clientes",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Clientes",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Clientes",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "PetShops",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "PetShops",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "Entregadores",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Entregadores",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
