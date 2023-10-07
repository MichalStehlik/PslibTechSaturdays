using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PslibTechSaturdays.Migrations
{
    /// <inheritdoc />
    public partial class Presence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Present",
                table: "Enrollments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Actions",
                keyColumn: "ActionId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 7, 16, 30, 57, 523, DateTimeKind.Local).AddTicks(5329));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "7fbcb67b-69e8-402b-993a-3d89f90f193b", new DateTime(2023, 10, 7, 16, 30, 57, 467, DateTimeKind.Local).AddTicks(9117), "AQAAAAIAAYagAAAAED8+N1NR8+10fhhu1rX1EZPeUOojO7FbUcdunrbLs9+H+LHRjbH4weLgGUGmJ3EXaQ==", new DateTime(2023, 10, 7, 16, 30, 57, 467, DateTimeKind.Local).AddTicks(9216) });

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 7, 16, 30, 57, 525, DateTimeKind.Local).AddTicks(6945));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 10, 7, 16, 30, 57, 525, DateTimeKind.Local).AddTicks(6987));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Present",
                table: "Enrollments",
                type: "bit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
