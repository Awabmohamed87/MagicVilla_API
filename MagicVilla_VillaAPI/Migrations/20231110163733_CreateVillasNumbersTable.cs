using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateVillasNumbersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VillasNumbers",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    SpecialDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillasNumbers", x => x.VillaNo);
                });

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 10, 18, 37, 33, 678, DateTimeKind.Local).AddTicks(1840));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 10, 18, 37, 33, 678, DateTimeKind.Local).AddTicks(1899));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 10, 18, 37, 33, 678, DateTimeKind.Local).AddTicks(1903));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 10, 18, 37, 33, 678, DateTimeKind.Local).AddTicks(1959));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 10, 18, 37, 33, 678, DateTimeKind.Local).AddTicks(1963));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillasNumbers");

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 6, 13, 16, 58, 572, DateTimeKind.Local).AddTicks(8682));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 6, 13, 16, 58, 572, DateTimeKind.Local).AddTicks(8788));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 6, 13, 16, 58, 572, DateTimeKind.Local).AddTicks(8792));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 6, 13, 16, 58, 572, DateTimeKind.Local).AddTicks(8795));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 6, 13, 16, 58, 572, DateTimeKind.Local).AddTicks(8798));
        }
    }
}
