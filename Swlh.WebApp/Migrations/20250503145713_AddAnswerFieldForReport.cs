using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAnswerFieldForReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "Report",
                type: "nvarchar(max)",
                maxLength: 65535,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Report");
        }
    }
}
