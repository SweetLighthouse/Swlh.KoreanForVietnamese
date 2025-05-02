using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameStuff2AndAddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessedAcount",
                table: "Account",
                newName: "AccessedCount");

            migrationBuilder.AddColumn<int>(
                name: "AccessedCount",
                table: "Report",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Report",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AccessedCount",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Post",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessedCount",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "AccessedCount",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "AccessedCount",
                table: "Account",
                newName: "AccessedAcount");
        }
    }
}
