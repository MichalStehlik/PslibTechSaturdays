using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PslibTechSaturdays.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApplicationCountVisible",
                table: "Groups",
                newName: "EnrollmentsCountVisible");

            migrationBuilder.UpdateData(
                table: "Actions",
                keyColumn: "ActionId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 9, 30, 10, 18, 52, 548, DateTimeKind.Local).AddTicks(6804));

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "ClaimType",
                value: "lector");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "6fdf9258-3dc0-4f65-ae93-6c714ac274ae", new DateTime(2023, 9, 30, 10, 18, 52, 512, DateTimeKind.Local).AddTicks(5541), "AQAAAAIAAYagAAAAEAsZW0/NJ6Wnyd0diHc0pGRffpgDsJV6BlcAE4+pRutzyy55foluIOyxosSY4zGXkw==", new DateTime(2023, 9, 30, 10, 18, 52, 512, DateTimeKind.Local).AddTicks(5595) });

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 9, 30, 10, 18, 52, 548, DateTimeKind.Local).AddTicks(6843));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 9, 30, 10, 18, 52, 548, DateTimeKind.Local).AddTicks(6851));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnrollmentsCountVisible",
                table: "Groups",
                newName: "ApplicationCountVisible");

            migrationBuilder.UpdateData(
                table: "Actions",
                keyColumn: "ActionId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 9, 1, 0, 22, 0, 861, DateTimeKind.Local).AddTicks(9021));

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "ClaimType",
                value: "lektor");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "f8098904-e2d6-467d-a4a8-b86e456afa63", new DateTime(2023, 9, 1, 0, 22, 0, 827, DateTimeKind.Local).AddTicks(2747), "AQAAAAIAAYagAAAAEJ9QdGZZ3TCenUIjqjFtsgZ7tvwniYehWODSYaljUdHIqS+eQ+MZ95+AxZWJlT317w==", new DateTime(2023, 9, 1, 0, 22, 0, 827, DateTimeKind.Local).AddTicks(2855) });

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 9, 1, 0, 22, 0, 861, DateTimeKind.Local).AddTicks(9061));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 9, 1, 0, 22, 0, 861, DateTimeKind.Local).AddTicks(9068));
        }
    }
}
