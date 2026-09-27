using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Libro",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    autor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    catalogo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libro", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    correo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    usuario = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    contrasena = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    rol = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Prestamo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioId = table.Column<int>(type: "int", nullable: false),
                    libroId = table.Column<int>(type: "int", nullable: false),
                    fechaPrestamo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fechaDevolucion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false),
                    UsuariosId = table.Column<int>(type: "int", nullable: true),
                    LibrosId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamo", x => x.id);
                    table.ForeignKey(
                        name: "FK_Prestamo_Libro_LibrosId",
                        column: x => x.LibrosId,
                        principalTable: "Libro",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Prestamo_Usuario_UsuariosId",
                        column: x => x.UsuariosId,
                        principalTable: "Usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prestamo_LibrosId",
                table: "Prestamo",
                column: "LibrosId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamo_UsuariosId",
                table: "Prestamo",
                column: "UsuariosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prestamo");

            migrationBuilder.DropTable(
                name: "Libro");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
