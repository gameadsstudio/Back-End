using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class GameMiniature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "miniature_url",
                table: "game",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "miniature_url",
                table: "game");
        }
    }
}
