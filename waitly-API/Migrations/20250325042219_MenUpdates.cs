using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace waitly_API.Migrations
{
    /// <inheritdoc />
    public partial class MenUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Catalogos_TipoId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_TipoId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "TipoId",
                table: "Menus");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_IdTipo",
                table: "Menus",
                column: "IdTipo");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Catalogos_IdTipo",
                table: "Menus",
                column: "IdTipo",
                principalTable: "Catalogos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Catalogos_IdTipo",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_IdTipo",
                table: "Menus");

            migrationBuilder.AddColumn<int>(
                name: "TipoId",
                table: "Menus",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_TipoId",
                table: "Menus",
                column: "TipoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Catalogos_TipoId",
                table: "Menus",
                column: "TipoId",
                principalTable: "Catalogos",
                principalColumn: "Id");
        }
    }
}
