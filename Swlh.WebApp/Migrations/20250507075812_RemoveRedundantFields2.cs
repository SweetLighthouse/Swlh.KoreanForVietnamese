using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRedundantFields2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "CommentOnPost");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "CommentOnKeyword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "CommentOnPost",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "CommentOnKeyword",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
