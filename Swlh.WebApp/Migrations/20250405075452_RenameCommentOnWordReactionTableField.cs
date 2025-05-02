using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameCommentOnWordReactionTableField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentOnWordReaction_CommentOnWord_CommentOnWordId",
                table: "CommentOnWordReaction");

            migrationBuilder.RenameColumn(
                name: "CommentOnWordId",
                table: "CommentOnWordReaction",
                newName: "CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentOnWordReaction_CommentOnWordId",
                table: "CommentOnWordReaction",
                newName: "IX_CommentOnWordReaction_CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentOnWordReaction_CommentOnWord_CommentId",
                table: "CommentOnWordReaction",
                column: "CommentId",
                principalTable: "CommentOnWord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentOnWordReaction_CommentOnWord_CommentId",
                table: "CommentOnWordReaction");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "CommentOnWordReaction",
                newName: "CommentOnWordId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentOnWordReaction_CommentId",
                table: "CommentOnWordReaction",
                newName: "IX_CommentOnWordReaction_CommentOnWordId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentOnWordReaction_CommentOnWord_CommentOnWordId",
                table: "CommentOnWordReaction",
                column: "CommentOnWordId",
                principalTable: "CommentOnWord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
