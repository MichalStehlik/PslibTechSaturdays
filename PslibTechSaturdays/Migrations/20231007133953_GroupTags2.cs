using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PslibTechSaturdays.Migrations
{
    /// <inheritdoc />
    public partial class GroupTags2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupTag");

            migrationBuilder.CreateTable(
                name: "GroupTags",
                columns: table => new
                {
                    GroupsGroupId = table.Column<int>(type: "int", nullable: false),
                    TagsTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTags", x => new { x.GroupsGroupId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_GroupTags_Groups_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupTags_Tags_TagsTagId",
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
                value: new DateTime(2023, 10, 7, 15, 39, 53, 850, DateTimeKind.Local).AddTicks(1358));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "38cc3def-eb63-4615-815d-f4e1359ddcbf", new DateTime(2023, 10, 7, 15, 39, 53, 814, DateTimeKind.Local).AddTicks(8746), "AQAAAAIAAYagAAAAEMsv8auFCMGwB3yijaEzMwCfrl4ugFaGSeYGflZRlEUGEdykbpP1YozIiRSm7+QDnA==", new DateTime(2023, 10, 7, 15, 39, 53, 814, DateTimeKind.Local).AddTicks(8798) });

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 10, 7, 15, 39, 53, 850, DateTimeKind.Local).AddTicks(7146));

            migrationBuilder.UpdateData(
                table: "Groups",
                keyColumn: "GroupId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 10, 7, 15, 39, 53, 850, DateTimeKind.Local).AddTicks(7161));

            migrationBuilder.CreateIndex(
                name: "IX_GroupTags_TagsTagId",
                table: "GroupTags",
                column: "TagsTagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupTags");

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

            migrationBuilder.CreateIndex(
                name: "IX_GroupTag_TagsTagId",
                table: "GroupTag",
                column: "TagsTagId");
        }
    }
}
