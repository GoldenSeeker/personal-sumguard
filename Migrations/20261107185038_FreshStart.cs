using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Personal_Sumguard.Migrations
{
    /// <inheritdoc />
    public partial class FreshStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecallRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecallNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    RecallDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecallRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewDrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsShared = table.Column<bool>(type: "INTEGER", nullable: false),
                    RecallRecordId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewDrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewDrafts_RecallRecords_RecallRecordId",
                        column: x => x.RecallRecordId,
                        principalTable: "RecallRecords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReviewShares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReviewDraftId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerEmail = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SharedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewShares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewShares_ReviewDrafts_ReviewDraftId",
                        column: x => x.ReviewDraftId,
                        principalTable: "ReviewDrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDrafts_RecallRecordId",
                table: "ReviewDrafts",
                column: "RecallRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewShares_ReviewDraftId",
                table: "ReviewShares",
                column: "ReviewDraftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewShares");

            migrationBuilder.DropTable(
                name: "ReviewDrafts");

            migrationBuilder.DropTable(
                name: "RecallRecords");
        }
    }
}
