using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PslibTechSaturdays.Migrations
{
    /// <inheritdoc />
    public partial class GroupTags3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupTags_Groups_GroupsGroupId",
                table: "GroupTags");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupTags_Tags_TagsTagId",
                table: "GroupTags");

            migrationBuilder.RenameColumn(
                name: "TagsTagId",
                table: "GroupTags",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "GroupsGroupId",
                table: "GroupTags",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupTags_TagsTagId",
                table: "GroupTags",
                newName: "IX_GroupTags_TagId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTags_Groups_GroupId",
                table: "GroupTags",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTags_Tags_TagId",
                table: "GroupTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupTags_Groups_GroupId",
                table: "GroupTags");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupTags_Tags_TagId",
                table: "GroupTags");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "GroupTags",
                newName: "TagsTagId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "GroupTags",
                newName: "GroupsGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupTags_TagId",
                table: "GroupTags",
                newName: "IX_GroupTags_TagsTagId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTags_Groups_GroupsGroupId",
                table: "GroupTags",
                column: "GroupsGroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTags_Tags_TagsTagId",
                table: "GroupTags",
                column: "TagsTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
