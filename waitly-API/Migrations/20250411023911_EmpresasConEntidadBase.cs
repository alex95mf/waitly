using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace waitly_API.Migrations
{
    /// <inheritdoc />
    public partial class EmpresasConEntidadBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Empresas",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEliminacion",
                table: "Empresas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Empresas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpCreacion",
                table: "Empresas",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpEliminacion",
                table: "Empresas",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpModificacion",
                table: "Empresas",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Removido",
                table: "Empresas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Empresas",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacion",
                table: "Empresas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioEliminacion",
                table: "Empresas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "Empresas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "FechaEliminacion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "IpCreacion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "IpEliminacion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "IpModificacion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Removido",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "UsuarioEliminacion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "Empresas");
        }
    }
}
