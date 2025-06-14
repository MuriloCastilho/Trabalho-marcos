using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication4.Migrations
{
    /// <inheritdoc />
    public partial class funciona_caralho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Descontos_Medicamentos_PrincipioAtivo",
                table: "Descontos");

            migrationBuilder.DropIndex(
                name: "IX_Descontos_PrincipioAtivo",
                table: "Descontos");

            migrationBuilder.CreateIndex(
                name: "IX_Descontos_MedicamentoId",
                table: "Descontos",
                column: "MedicamentoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Descontos_Medicamentos_MedicamentoId",
                table: "Descontos",
                column: "MedicamentoId",
                principalTable: "Medicamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Descontos_Medicamentos_MedicamentoId",
                table: "Descontos");

            migrationBuilder.DropIndex(
                name: "IX_Descontos_MedicamentoId",
                table: "Descontos");

            migrationBuilder.CreateIndex(
                name: "IX_Descontos_PrincipioAtivo",
                table: "Descontos",
                column: "PrincipioAtivo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Descontos_Medicamentos_PrincipioAtivo",
                table: "Descontos",
                column: "PrincipioAtivo",
                principalTable: "Medicamentos",
                principalColumn: "PrincipioAtivo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
