using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace waitly_API.Migrations
{
    /// <inheritdoc />
    public partial class MenuIdPermisoEsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Permisos_IdPermiso",
                table: "Menus");

            migrationBuilder.AlterColumn<int>(
                name: "IdPermiso",
                table: "Menus",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Permisos_IdPermiso",
                table: "Menus",
                column: "IdPermiso",
                principalTable: "Permisos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Permisos_IdPermiso",
                table: "Menus");

            migrationBuilder.AlterColumn<int>(
                name: "IdPermiso",
                table: "Menus",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Permisos_IdPermiso",
                table: "Menus",
                column: "IdPermiso",
                principalTable: "Permisos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
