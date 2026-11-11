using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Personal_Sumguard.Migrations
{
    /// <inheritdoc />
    public partial class ExtendRecallRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClassificationDate",
                table: "RecallRecords",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirmName",
                table: "RecallRecords",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostedDate",
                table: "RecallRecords",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "RecallRecords",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecallClass",
                table: "RecallRecords",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecallReason",
                table: "RecallRecords",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TerminationDate",
                table: "RecallRecords",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TradeName",
                table: "RecallRecords",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassificationDate",
                table: "RecallRecords");

            migrationBuilder.DropColumn(
                name: "FirmName",
                table: "RecallRecords");

            migrationBuilder.DropColumn(
                name: "PostedDate",
                table: "RecallRecords");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "RecallRecords");

            migrationBuilder.DropColumn(
                name: "RecallClass",
                table: "RecallRecords");

            migrationBuilder.DropColumn(
                name: "RecallReason",
                table: "RecallRecords");

            migrationBuilder.DropColumn(
                name: "TerminationDate",
                table: "RecallRecords");

            migrationBuilder.DropColumn(
                name: "TradeName",
                table: "RecallRecords");
        }
    }
}
