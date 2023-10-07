using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PslibTechSaturdays.Migrations
{
    /// <inheritdoc />
    public partial class EnrollmentFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CancelledById",
                table: "Enrollments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "Actions",
                keyColumn: "ActionId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 7, 16, 4, 17, 302, DateTimeKind.Local).AddTicks(2432));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "a6680a20-a7b7-44aa-9fde-4c9f05b27801", new DateTime(2023, 10, 7, 16, 4, 17, 266, DateTimeKind.Local).AddTicks(7768), "AQAAAAIAAYagAAAAEDSIcUhjXhDo6OsUx2aYAryIw52MzKcuMsqaaHIrVjfIbjCdsFrdr4NqC5QqAPVdCw==", new DateTime(2023, 10, 7, 16, 4, 17, 266, DateTimeKind.Local).AddTicks(7828) });

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 7, 16, 4, 17, 303, DateTimeKind.Local).AddTicks(2188));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 10, 7, 16, 4, 17, 303, DateTimeKind.Local).AddTicks(2205));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CancelledById",
                table: "Enrollments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Actions",
                keyColumn: "ActionId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 7, 15, 44, 12, 208, DateTimeKind.Local).AddTicks(356));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "889b81fc-ea9f-41f4-b9be-e773a292ac25", new DateTime(2023, 10, 7, 15, 44, 12, 172, DateTimeKind.Local).AddTicks(3550), "AQAAAAIAAYagAAAAEAQpNHXRV8e+rsqn+TXLi4H5NjFC2fflRixxS+FjXaVIndGnCTdz/CykK5gKMUAU4g==", new DateTime(2023, 10, 7, 15, 44, 12, 172, DateTimeKind.Local).AddTicks(3598) });

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 7, 15, 44, 12, 209, DateTimeKind.Local).AddTicks(1735));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 10, 7, 15, 44, 12, 209, DateTimeKind.Local).AddTicks(1756));
        }
    }
}
