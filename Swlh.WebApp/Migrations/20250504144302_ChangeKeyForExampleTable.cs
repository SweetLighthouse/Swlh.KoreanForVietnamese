using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKeyForExampleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Example",
                table: "Example");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Example",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("UPDATE Example SET Id = NEWID() WHERE Id = '00000000-0000-0000-0000-000000000000'");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Example",
                table: "Example",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Example",
                table: "Example");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Example");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Example",
                table: "Example",
                columns: new[] { "Korean", "Vietnamese" });
        }
    }
}
