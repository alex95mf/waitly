using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace waitly_API.Migrations
{
    /// <inheritdoc />
    public partial class ClasesConEntidadBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Roles",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEliminacion",
                table: "Roles",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Roles",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpCreacion",
                table: "Roles",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpEliminacion",
                table: "Roles",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpModificacion",
                table: "Roles",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Removido",
                table: "Roles",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Roles",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacion",
                table: "Roles",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioEliminacion",
                table: "Roles",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "Roles",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Permisos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEliminacion",
                table: "Permisos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Permisos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpCreacion",
                table: "Permisos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpEliminacion",
                table: "Permisos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpModificacion",
                table: "Permisos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Removido",
                table: "Permisos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Permisos",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacion",
                table: "Permisos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioEliminacion",
                table: "Permisos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "Permisos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Pantallas",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEliminacion",
                table: "Pantallas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Pantallas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpCreacion",
                table: "Pantallas",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpEliminacion",
                table: "Pantallas",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpModificacion",
                table: "Pantallas",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Removido",
                table: "Pantallas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Pantallas",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacion",
                table: "Pantallas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioEliminacion",
                table: "Pantallas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "Pantallas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Menus",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEliminacion",
                table: "Menus",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Menus",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpCreacion",
                table: "Menus",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpEliminacion",
                table: "Menus",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpModificacion",
                table: "Menus",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Removido",
                table: "Menus",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Menus",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacion",
                table: "Menus",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioEliminacion",
                table: "Menus",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "Menus",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "GruposAsientos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEliminacion",
                table: "GruposAsientos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "GruposAsientos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpCreacion",
                table: "GruposAsientos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpEliminacion",
                table: "GruposAsientos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpModificacion",
                table: "GruposAsientos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Removido",
                table: "GruposAsientos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "GruposAsientos",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacion",
                table: "GruposAsientos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioEliminacion",
                table: "GruposAsientos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "GruposAsientos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Catalogos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEliminacion",
                table: "Catalogos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Catalogos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpCreacion",
                table: "Catalogos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpEliminacion",
                table: "Catalogos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpModificacion",
                table: "Catalogos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Removido",
                table: "Catalogos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Catalogos",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacion",
                table: "Catalogos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioEliminacion",
                table: "Catalogos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "Catalogos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Asientos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEliminacion",
                table: "Asientos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "Asientos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpCreacion",
                table: "Asientos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpEliminacion",
                table: "Asientos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "IpModificacion",
                table: "Asientos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Removido",
                table: "Asientos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Asientos",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacion",
                table: "Asientos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioEliminacion",
                table: "Asientos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "Asientos",
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
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "FechaEliminacion",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IpCreacion",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IpEliminacion",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IpModificacion",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Removido",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacion",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UsuarioEliminacion",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "FechaEliminacion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "IpCreacion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "IpEliminacion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "IpModificacion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "Removido",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "UsuarioEliminacion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Pantallas");

            migrationBuilder.DropColumn(
                name: "FechaEliminacion",
                table: "Pantallas");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Pantallas");

            migrationBuilder.DropColumn(
                name: "IpCreacion",
                table: "Pantallas");

            migrationBuilder.DropColumn(
                name: "IpEliminacion",
                table: "Pantallas");

            migrationBuilder.DropColumn(
                name: "IpModificacion",
                table: "Pantallas");

            migrationBuilder.DropColumn(
                name: "Removido",
                table: "Pantallas");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Pantallas");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacion",
                table: "Pantallas");

            migrationBuilder.DropColumn(
                name: "UsuarioEliminacion",
                table: "Pantallas");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "Pantallas");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "FechaEliminacion",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "IpCreacion",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "IpEliminacion",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "IpModificacion",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Removido",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacion",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UsuarioEliminacion",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "GruposAsientos");

            migrationBuilder.DropColumn(
                name: "FechaEliminacion",
                table: "GruposAsientos");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "GruposAsientos");

            migrationBuilder.DropColumn(
                name: "IpCreacion",
                table: "GruposAsientos");

            migrationBuilder.DropColumn(
                name: "IpEliminacion",
                table: "GruposAsientos");

            migrationBuilder.DropColumn(
                name: "IpModificacion",
                table: "GruposAsientos");

            migrationBuilder.DropColumn(
                name: "Removido",
                table: "GruposAsientos");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "GruposAsientos");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacion",
                table: "GruposAsientos");

            migrationBuilder.DropColumn(
                name: "UsuarioEliminacion",
                table: "GruposAsientos");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "GruposAsientos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Catalogos");

            migrationBuilder.DropColumn(
                name: "FechaEliminacion",
                table: "Catalogos");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Catalogos");

            migrationBuilder.DropColumn(
                name: "IpCreacion",
                table: "Catalogos");

            migrationBuilder.DropColumn(
                name: "IpEliminacion",
                table: "Catalogos");

            migrationBuilder.DropColumn(
                name: "IpModificacion",
                table: "Catalogos");

            migrationBuilder.DropColumn(
                name: "Removido",
                table: "Catalogos");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Catalogos");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacion",
                table: "Catalogos");

            migrationBuilder.DropColumn(
                name: "UsuarioEliminacion",
                table: "Catalogos");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "Catalogos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Asientos");

            migrationBuilder.DropColumn(
                name: "FechaEliminacion",
                table: "Asientos");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "Asientos");

            migrationBuilder.DropColumn(
                name: "IpCreacion",
                table: "Asientos");

            migrationBuilder.DropColumn(
                name: "IpEliminacion",
                table: "Asientos");

            migrationBuilder.DropColumn(
                name: "IpModificacion",
                table: "Asientos");

            migrationBuilder.DropColumn(
                name: "Removido",
                table: "Asientos");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Asientos");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacion",
                table: "Asientos");

            migrationBuilder.DropColumn(
                name: "UsuarioEliminacion",
                table: "Asientos");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "Asientos");
        }
    }
}
