using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PslibTechSaturdays.Migrations
{
    /// <inheritdoc />
    public partial class GroupTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackgroundColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForegroundColor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "GroupTag",
                columns: table => new
                {
                    GroupsGroupId = table.Column<int>(type: "int", nullable: false),
                    TagsTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTag", x => new { x.GroupsGroupId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_GroupTag_Groups_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupTag_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Actions",
                keyColumn: "ActionId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 7, 15, 8, 31, 26, DateTimeKind.Local).AddTicks(258));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "ebfa7733-fc69-4713-9753-b3334456486f", new DateTime(2023, 10, 7, 15, 8, 30, 988, DateTimeKind.Local).AddTicks(2830), "AQAAAAIAAYagAAAAEIO/IeWQGtUEDFwhaQ+HC7UF6guxsZN7JxTHKuQ30Z1i4h9utGLpB0AiywK2VvVxmQ==", new DateTime(2023, 10, 7, 15, 8, 30, 988, DateTimeKind.Local).AddTicks(2883) });

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 7, 15, 8, 31, 26, DateTimeKind.Local).AddTicks(333));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 10, 7, 15, 8, 31, 26, DateTimeKind.Local).AddTicks(343));

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "TagId", "BackgroundColor", "ForegroundColor", "Text" },
                values: new object[,]
                {
                    { 1, "#e34242", "#ffffff", "IT" },
                    { 2, "#429fe3", "#ffffff", "Strojírenství" },
                    { 3, "#3cab68", "#ffffff", "Elektrotechnika" },
                    { 4, "#e3a342", "#ffffff", "Lyceum" },
                    { 5, "#9c42e3", "#ffffff", "Oděvnictví" },
                    { 6, "#e3428f", "#ffffff", "Textilnictví" },
                    { 7, "#436a68", "#ffffff", "VOŠ" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupTag_TagsTagId",
                table: "GroupTag",
                column: "TagsTagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupTag");

            migrationBuilder.DropTable(
                name: "Tags");

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
        }
    }
}
