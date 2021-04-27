using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class ChangeOrganizationsDateModificationToDateUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date_modification",
                table: "organization",
                newName: "date_update");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date_update",
                table: "organization",
                newName: "date_modification");
        }
    }
}
