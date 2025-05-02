using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipForCommentOnPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Account_AccountId",
                table: "Post");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Post",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "CommentOnPost",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "CommentOnPost",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnPost_AccountId",
                table: "CommentOnPost",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnPost_PostId",
                table: "CommentOnPost",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentOnPost_Account_AccountId",
                table: "CommentOnPost",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentOnPost_Post_PostId",
                table: "CommentOnPost",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Account_AccountId",
                table: "Post",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentOnPost_Account_AccountId",
                table: "CommentOnPost");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentOnPost_Post_PostId",
                table: "CommentOnPost");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Account_AccountId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_CommentOnPost_AccountId",
                table: "CommentOnPost");

            migrationBuilder.DropIndex(
                name: "IX_CommentOnPost_PostId",
                table: "CommentOnPost");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "CommentOnPost");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "CommentOnPost");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Post",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Account_AccountId",
                table: "Post",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
