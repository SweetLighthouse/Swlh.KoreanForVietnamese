using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipForLikeDislikeCommentOnWord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CommentOnWord_LikeDislike_CommentOnWordId",
                table: "CommentOnWord_LikeDislike",
                column: "CommentOnWordId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentOnWord_LikeDislike_Account_AccountId",
                table: "CommentOnWord_LikeDislike",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentOnWord_LikeDislike_CommentOnWord_CommentOnWordId",
                table: "CommentOnWord_LikeDislike",
                column: "CommentOnWordId",
                principalTable: "CommentOnWord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentOnWord_LikeDislike_Account_AccountId",
                table: "CommentOnWord_LikeDislike");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentOnWord_LikeDislike_CommentOnWord_CommentOnWordId",
                table: "CommentOnWord_LikeDislike");

            migrationBuilder.DropIndex(
                name: "IX_CommentOnWord_LikeDislike_CommentOnWordId",
                table: "CommentOnWord_LikeDislike");
        }
    }
}
