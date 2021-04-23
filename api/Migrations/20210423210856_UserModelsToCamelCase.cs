using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class UserModelsToCamelCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "user",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "firstname",
                table: "user",
                newName: "first_name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "user",
                newName: "lastname");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "user",
                newName: "firstname");
        }
    }
}
