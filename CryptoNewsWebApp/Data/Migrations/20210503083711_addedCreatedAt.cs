using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoNewsWebApp.Data.Migrations
{
    public partial class addedCreatedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateTime",
                table: "Post");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Subreddit",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Post",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Subreddit");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Post");

            migrationBuilder.AddColumn<DateTime>(
                name: "dateTime",
                table: "Post",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
