using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoNewsWebApp.Data.Migrations
{
    public partial class ChangedModelsAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Source_DataSourceId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "RedditApp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Source",
                table: "Source");

            migrationBuilder.RenameTable(
                name: "Source",
                newName: "DataSource");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataSource",
                table: "DataSource",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_DataSource_DataSourceId",
                table: "Post",
                column: "DataSourceId",
                principalTable: "DataSource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_DataSource_DataSourceId",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataSource",
                table: "DataSource");

            migrationBuilder.RenameTable(
                name: "DataSource",
                newName: "Source");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Source",
                table: "Source",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RedditApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RedditAppId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedditAppName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedditAppSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedditRefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedditApp", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Source_DataSourceId",
                table: "Post",
                column: "DataSourceId",
                principalTable: "Source",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
