using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoPrimerasFilas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImageUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa", new DateTime(2023, 7, 25, 21, 44, 40, 629, DateTimeKind.Local).AddTicks(8607), new DateTime(2023, 7, 25, 21, 44, 40, 629, DateTimeKind.Local).AddTicks(8585), "", 20, "Villa real", 5, 200.0 },
                    { 2, "", "Segundo detalle de la villa", new DateTime(2023, 7, 25, 21, 44, 40, 629, DateTimeKind.Local).AddTicks(8613), new DateTime(2023, 7, 25, 21, 44, 40, 629, DateTimeKind.Local).AddTicks(8611), "", 100, "Premiun Vista a la Piscina", 2, 20000.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
