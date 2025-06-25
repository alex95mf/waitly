using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace waitly_API.Migrations
{
    /// <inheritdoc />
    public partial class PermisoUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_Catalogos_TipoId",
                table: "Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_TipoId",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "TipoId",
                table: "Permisos");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_IdTipo",
                table: "Permisos",
                column: "IdTipo");

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_Catalogos_IdTipo",
                table: "Permisos",
                column: "IdTipo",
                principalTable: "Catalogos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_Catalogos_IdTipo",
                table: "Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_IdTipo",
                table: "Permisos");

            migrationBuilder.AddColumn<int>(
                name: "TipoId",
                table: "Permisos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_TipoId",
                table: "Permisos",
                column: "TipoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_Catalogos_TipoId",
                table: "Permisos",
                column: "TipoId",
                principalTable: "Catalogos",
                principalColumn: "Id");
        }
    }
}
