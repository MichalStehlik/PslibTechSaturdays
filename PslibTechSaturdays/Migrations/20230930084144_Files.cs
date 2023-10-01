using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PslibTechSaturdays.Migrations
{
    /// <inheritdoc />
    public partial class Files : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_Files_AspNetUsers_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Actions",
                keyColumn: "ActionId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 9, 30, 10, 41, 44, 478, DateTimeKind.Local).AddTicks(7209));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "324aa0f6-c1dc-4a5e-b43e-b8897aaad985", new DateTime(2023, 9, 30, 10, 41, 44, 444, DateTimeKind.Local).AddTicks(2789), "AQAAAAIAAYagAAAAEHNCNQ19mbpLXm0oeTkShwhRCmLwsyS0SGTRfKOekKB/fmm4ySZCFbG/F6wQTHIGDg==", new DateTime(2023, 9, 30, 10, 41, 44, 444, DateTimeKind.Local).AddTicks(2838) });

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 9, 30, 10, 41, 44, 478, DateTimeKind.Local).AddTicks(7241));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 9, 30, 10, 41, 44, 478, DateTimeKind.Local).AddTicks(7248));

            migrationBuilder.CreateIndex(
                name: "IX_Files_UploaderId",
                table: "Files",
                column: "UploaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.UpdateData(
                table: "Actions",
                keyColumn: "ActionId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 9, 30, 10, 18, 52, 548, DateTimeKind.Local).AddTicks(6804));

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
    }
}
