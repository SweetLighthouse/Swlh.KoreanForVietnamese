using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentOnPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Body",
                table: "CommentOnPost",
                newName: "Content");

            migrationBuilder.AddColumn<int>(
                name: "AccessedCount",
                table: "CommentOnPost",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CommentOnPost",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CommentOnPost",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessedCount",
                table: "CommentOnPost");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CommentOnPost");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CommentOnPost");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "CommentOnPost",
                newName: "Body");
        }
    }
}
