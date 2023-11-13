using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyToVillaNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VillaId",
                table: "VillasNumbers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 13, 10, 14, 5, 853, DateTimeKind.Local).AddTicks(2782));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 13, 10, 14, 5, 853, DateTimeKind.Local).AddTicks(2838));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 13, 10, 14, 5, 853, DateTimeKind.Local).AddTicks(2843));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 13, 10, 14, 5, 853, DateTimeKind.Local).AddTicks(2846));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 13, 10, 14, 5, 853, DateTimeKind.Local).AddTicks(2850));

            migrationBuilder.CreateIndex(
                name: "IX_VillasNumbers_VillaId",
                table: "VillasNumbers",
                column: "VillaId");

            migrationBuilder.AddForeignKey(
                name: "FK_VillasNumbers_Villas_Api_VillaId",
                table: "VillasNumbers",
                column: "VillaId",
                principalTable: "Villas_Api",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VillasNumbers_Villas_Api_VillaId",
                table: "VillasNumbers");

            migrationBuilder.DropIndex(
                name: "IX_VillasNumbers_VillaId",
                table: "VillasNumbers");

            migrationBuilder.DropColumn(
                name: "VillaId",
                table: "VillasNumbers");

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
    }
}
