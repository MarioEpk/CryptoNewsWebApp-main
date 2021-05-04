using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoNewsWebApp.Data.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "JokeQuestion",
                table: "Joke",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RedditApp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RedditAppName = table.Column<string>(nullable: true),
                    RedditAppId = table.Column<string>(nullable: true),
                    RedditAppSecret = table.Column<string>(nullable: true),
                    RedditRefreshToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedditApp", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedditApp");

            migrationBuilder.AlterColumn<string>(
                name: "JokeQuestion",
                table: "Joke",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
