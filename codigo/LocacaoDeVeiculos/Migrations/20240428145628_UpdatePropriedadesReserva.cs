using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocacaoDeVeiculos.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePropriedadesReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IDVeiculo",
                table: "Reservas",
                newName: "VeiculoID");

            migrationBuilder.RenameColumn(
                name: "IDCliente",
                table: "Reservas",
                newName: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_ClienteID",
                table: "Reservas",
                column: "ClienteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Clientes_ClienteID",
                table: "Reservas",
                column: "ClienteID",
                principalTable: "Clientes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Clientes_ClienteID",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_ClienteID",
                table: "Reservas");

            migrationBuilder.RenameColumn(
                name: "VeiculoID",
                table: "Reservas",
                newName: "IDVeiculo");

            migrationBuilder.RenameColumn(
                name: "ClienteID",
                table: "Reservas",
                newName: "IDCliente");
        }
    }
}
