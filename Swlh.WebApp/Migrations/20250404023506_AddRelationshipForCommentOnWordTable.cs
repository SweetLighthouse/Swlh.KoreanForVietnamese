using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipForCommentOnWordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "CommentOnWord",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnWord_AccountId",
                table: "CommentOnWord",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnWord_WordId",
                table: "CommentOnWord",
                column: "WordId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentOnWord_Account_AccountId",
                table: "CommentOnWord",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentOnWord_Word_WordId",
                table: "CommentOnWord",
                column: "WordId",
                principalTable: "Word",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentOnWord_Account_AccountId",
                table: "CommentOnWord");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentOnWord_Word_WordId",
                table: "CommentOnWord");

            migrationBuilder.DropIndex(
                name: "IX_CommentOnWord_AccountId",
                table: "CommentOnWord");

            migrationBuilder.DropIndex(
                name: "IX_CommentOnWord_WordId",
                table: "CommentOnWord");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "CommentOnWord",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
