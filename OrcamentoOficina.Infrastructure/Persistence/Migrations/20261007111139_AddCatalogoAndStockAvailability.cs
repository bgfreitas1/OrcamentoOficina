using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrcamentoOficina.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCatalogoAndStockAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisponivelEmEstoque",
                table: "ItensOrcamento",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ItensCatalogo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensCatalogo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensCatalogo_Codigo",
                table: "ItensCatalogo",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensCatalogo_Tipo_Ativo",
                table: "ItensCatalogo",
                columns: new[] { "Tipo", "Ativo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensCatalogo");

            migrationBuilder.DropColumn(
                name: "DisponivelEmEstoque",
                table: "ItensOrcamento");
        }
    }
}
