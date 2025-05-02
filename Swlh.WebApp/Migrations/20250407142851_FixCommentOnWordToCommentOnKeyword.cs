using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swlh.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class FixCommentOnWordToCommentOnKeyword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentOnWordReaction");

            migrationBuilder.DropTable(
                name: "KeywordSearchCount");

            migrationBuilder.DropTable(
                name: "CommentOnWord");

            migrationBuilder.CreateTable(
                name: "Keyword",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SearchCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "CommentOnKeyword",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1023)", maxLength: 1023, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOnKeyword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentOnKeyword_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CommentOnKeyword_Keyword_Key",
                        column: x => x.Key,
                        principalTable: "Keyword",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentOnKeywordReaction",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOnKeywordReaction", x => new { x.AccountId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_CommentOnKeywordReaction_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentOnKeywordReaction_CommentOnKeyword_CommentId",
                        column: x => x.CommentId,
                        principalTable: "CommentOnKeyword",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnKeyword_AccountId",
                table: "CommentOnKeyword",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnKeyword_Key",
                table: "CommentOnKeyword",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnKeywordReaction_CommentId",
                table: "CommentOnKeywordReaction",
                column: "CommentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentOnKeywordReaction");

            migrationBuilder.DropTable(
                name: "CommentOnKeyword");

            migrationBuilder.DropTable(
                name: "Keyword");

            migrationBuilder.CreateTable(
                name: "CommentOnWord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1023)", maxLength: 1023, nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOnWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentOnWord_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CommentOnWord_Word_WordId",
                        column: x => x.WordId,
                        principalTable: "Word",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeywordSearchCount",
                columns: table => new
                {
                    Keyword = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SearchCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordSearchCount", x => x.Keyword);
                });

            migrationBuilder.CreateTable(
                name: "CommentOnWordReaction",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOnWordReaction", x => new { x.AccountId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_CommentOnWordReaction_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentOnWordReaction_CommentOnWord_CommentId",
                        column: x => x.CommentId,
                        principalTable: "CommentOnWord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnWord_AccountId",
                table: "CommentOnWord",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnWord_WordId",
                table: "CommentOnWord",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentOnWordReaction_CommentId",
                table: "CommentOnWordReaction",
                column: "CommentId");
        }
    }
}
