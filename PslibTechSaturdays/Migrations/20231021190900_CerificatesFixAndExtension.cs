using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PslibTechSaturdays.Migrations
{
    /// <inheritdoc />
    public partial class CerificatesFixAndExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Certificates_CertificateId",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_CertificateId",
                table: "Enrollments");

            migrationBuilder.AddColumn<int>(
                name: "EnrollmentId",
                table: "Certificates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Certificates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Actions",
                keyColumn: "ActionId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 21, 21, 9, 0, 65, DateTimeKind.Local).AddTicks(3329));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "2a1fdc8c-7153-4b84-b29b-bd08675b4b90", new DateTime(2023, 10, 21, 21, 9, 0, 28, DateTimeKind.Local).AddTicks(7578), "AQAAAAIAAYagAAAAEIf2X2AxOxBhd+VOQnb00fzCw6FNGfcXDYvBRMSx6ZeNiLEIZUJqrOPzEr+G6BNRcg==", new DateTime(2023, 10, 21, 21, 9, 0, 28, DateTimeKind.Local).AddTicks(7649) });

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 21, 21, 9, 0, 66, DateTimeKind.Local).AddTicks(2887));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 10, 21, 21, 9, 0, 66, DateTimeKind.Local).AddTicks(2905));

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_EnrollmentId",
                table: "Certificates",
                column: "EnrollmentId",
                unique: true,
                filter: "[EnrollmentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Enrollments_EnrollmentId",
                table: "Certificates",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "EnrollmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Enrollments_EnrollmentId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_EnrollmentId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "EnrollmentId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Certificates");

            migrationBuilder.UpdateData(
                table: "Actions",
                keyColumn: "ActionId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 8, 23, 51, 59, 928, DateTimeKind.Local).AddTicks(9756));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "f11d9416-0796-40c1-861f-dda6d793e153", new DateTime(2023, 10, 8, 23, 51, 59, 893, DateTimeKind.Local).AddTicks(6710), "AQAAAAIAAYagAAAAED4x0GvCI88+sgwq1SqEnYAzIDoX9wyrbJ5XkdR2o+8P7RtZcmGO9vR2zI1qsQ5SoQ==", new DateTime(2023, 10, 8, 23, 51, 59, 893, DateTimeKind.Local).AddTicks(6786) });

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 8, 23, 51, 59, 929, DateTimeKind.Local).AddTicks(8094));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 10, 8, 23, 51, 59, 929, DateTimeKind.Local).AddTicks(8110));

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CertificateId",
                table: "Enrollments",
                column: "CertificateId",
                unique: true,
                filter: "[CertificateId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Certificates_CertificateId",
                table: "Enrollments",
                column: "CertificateId",
                principalTable: "Certificates",
                principalColumn: "CertificateId");
        }
    }
}
