using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentOnWord_LikeDislike");

            migrationBuilder.CreateTable(
                name: "CommentOnWordReaction",
                columns: table => new
                {
                    CommentOnWordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOnWordReaction", x => new { x.AccountId, x.CommentOnWordId });
                    table.ForeignKey(
                        name: "FK_CommentOnWordReaction_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentOnWordReaction_CommentOnWord_CommentOnWordId",
                        column: x => x.CommentOnWordId,
                        principalTable: "CommentOnWord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnWordReaction_CommentOnWordId",
                table: "CommentOnWordReaction",
                column: "CommentOnWordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentOnWordReaction");

            migrationBuilder.CreateTable(
                name: "CommentOnWord_LikeDislike",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentOnWordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOnWord_LikeDislike", x => new { x.AccountId, x.CommentOnWordId });
                    table.ForeignKey(
                        name: "FK_CommentOnWord_LikeDislike_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentOnWord_LikeDislike_CommentOnWord_CommentOnWordId",
                        column: x => x.CommentOnWordId,
                        principalTable: "CommentOnWord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnWord_LikeDislike_CommentOnWordId",
                table: "CommentOnWord_LikeDislike",
                column: "CommentOnWordId");
        }
    }
}
