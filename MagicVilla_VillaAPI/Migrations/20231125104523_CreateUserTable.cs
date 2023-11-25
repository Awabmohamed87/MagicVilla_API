using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalUsers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUsers", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 25, 12, 45, 23, 327, DateTimeKind.Local).AddTicks(6182));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 25, 12, 45, 23, 327, DateTimeKind.Local).AddTicks(6236));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 25, 12, 45, 23, 327, DateTimeKind.Local).AddTicks(6240));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 25, 12, 45, 23, 327, DateTimeKind.Local).AddTicks(6244));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 25, 12, 45, 23, 327, DateTimeKind.Local).AddTicks(6248));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalUsers");

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 15, 10, 27, 10, 504, DateTimeKind.Local).AddTicks(2322));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 15, 10, 27, 10, 504, DateTimeKind.Local).AddTicks(2374));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 15, 10, 27, 10, 504, DateTimeKind.Local).AddTicks(2378));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 15, 10, 27, 10, 504, DateTimeKind.Local).AddTicks(2381));

            migrationBuilder.UpdateData(
                table: "Villas_Api",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 11, 15, 10, 27, 10, 504, DateTimeKind.Local).AddTicks(2385));
        }
    }
}
