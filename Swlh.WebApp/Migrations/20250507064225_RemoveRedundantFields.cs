using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRedundantFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessedCount",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "AccessedCount",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "AccessedCount",
                table: "CustomFile");

            migrationBuilder.DropColumn(
                name: "AccessedCount",
                table: "CommentOnPost");

            migrationBuilder.DropColumn(
                name: "AccessedCount",
                table: "CommentOnKeyword");

            migrationBuilder.DropColumn(
                name: "AccessedCount",
                table: "Account");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessedCount",
                table: "Tag",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tag",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tag",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AccessedCount",
                table: "Report",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessedCount",
                table: "CustomFile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessedCount",
                table: "CommentOnPost",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessedCount",
                table: "CommentOnKeyword",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessedCount",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
