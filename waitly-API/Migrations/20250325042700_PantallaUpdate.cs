using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iSit_API.Migrations
{
    /// <inheritdoc />
    public partial class PantallaUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdPermiso",
                table: "Pantallas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdPermiso",
                table: "Pantallas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
