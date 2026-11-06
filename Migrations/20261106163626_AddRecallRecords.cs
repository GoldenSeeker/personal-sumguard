using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Personal_Sumguard.Migrations
{
    /// <inheritdoc />
    public partial class AddRecallRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecallRecordId",
                table: "ReviewDrafts",
                type: "INTEGER",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDrafts_RecallRecordId",
                table: "ReviewDrafts",
                column: "RecallRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDrafts_RecallRecords_RecallRecordId",
                table: "ReviewDrafts",
                column: "RecallRecordId",
                principalTable: "RecallRecords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDrafts_RecallRecords_RecallRecordId",
                table: "ReviewDrafts");

            migrationBuilder.DropTable(
                name: "RecallRecords");

            migrationBuilder.DropIndex(
                name: "IX_ReviewDrafts_RecallRecordId",
                table: "ReviewDrafts");

            migrationBuilder.DropColumn(
                name: "RecallRecordId",
                table: "ReviewDrafts");
        }
    }
}
