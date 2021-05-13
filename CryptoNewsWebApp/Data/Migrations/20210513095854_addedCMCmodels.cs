using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoNewsWebApp.Data.Migrations
{
    public partial class addedCMCmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeURL",
                table: "DataSource");

            migrationBuilder.AddColumn<int>(
                name: "CoinId",
                table: "DataSource",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Coin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    CMCRank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coin", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataSource_CoinId",
                table: "DataSource",
                column: "CoinId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSource_Coin_CoinId",
                table: "DataSource",
                column: "CoinId",
                principalTable: "Coin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSource_Coin_CoinId",
                table: "DataSource");

            migrationBuilder.DropTable(
                name: "Coin");

            migrationBuilder.DropIndex(
                name: "IX_DataSource_CoinId",
                table: "DataSource");

            migrationBuilder.DropColumn(
                name: "CoinId",
                table: "DataSource");

            migrationBuilder.AddColumn<string>(
                name: "HomeURL",
                table: "DataSource",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
