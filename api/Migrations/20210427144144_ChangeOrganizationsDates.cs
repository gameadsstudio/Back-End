using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class ChangeOrganizationsDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "modification_date",
                table: "organization",
                newName: "date_modification");

            migrationBuilder.RenameColumn(
                name: "creation_date",
                table: "organization",
                newName: "date_creation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date_modification",
                table: "organization",
                newName: "modification_date");

            migrationBuilder.RenameColumn(
                name: "date_creation",
                table: "organization",
                newName: "creation_date");
        }
    }
}
