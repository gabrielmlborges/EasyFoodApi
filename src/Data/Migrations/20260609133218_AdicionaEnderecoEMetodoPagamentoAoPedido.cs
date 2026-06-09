using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyFood.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaEnderecoEMetodoPagamentoAoPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnderecoEntrega",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetodoPagamento",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnderecoEntrega",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "MetodoPagamento",
                table: "Pedidos");
        }
    }
}
