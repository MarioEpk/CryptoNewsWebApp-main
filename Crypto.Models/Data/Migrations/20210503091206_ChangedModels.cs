using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Crypto.WebApplication.Data.Migrations
{
    public partial class ChangedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Subreddit_SubredditId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "Subreddit");

            migrationBuilder.DropIndex(
                name: "IX_Post_SubredditId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "SubredditId",
                table: "Post");

            migrationBuilder.AddColumn<int>(
                name: "DataSourceId",
                table: "Post",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Source",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    HomeURL = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    TypeOfSource = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Source", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_DataSourceId",
                table: "Post",
                column: "DataSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Source_DataSourceId",
                table: "Post",
                column: "DataSourceId",
                principalTable: "Source",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Source_DataSourceId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "Source");

            migrationBuilder.DropIndex(
                name: "IX_Post_DataSourceId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "DataSourceId",
                table: "Post");

            migrationBuilder.AddColumn<int>(
                name: "SubredditId",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subreddit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HomeURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subreddit", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_SubredditId",
                table: "Post",
                column: "SubredditId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Subreddit_SubredditId",
                table: "Post",
                column: "SubredditId",
                principalTable: "Subreddit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
