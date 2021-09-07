using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class frontfeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "alias",
                table: "user");

            migrationBuilder.DropColumn(
                name: "date_status",
                table: "user");

            migrationBuilder.DropColumn(
                name: "level",
                table: "user");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "user");

            migrationBuilder.DropColumn(
                name: "status",
                table: "user");

            migrationBuilder.DropColumn(
                name: "default_authorization",
                table: "organization");

            migrationBuilder.DropColumn(
                name: "localization",
                table: "organization");

            migrationBuilder.DropColumn(
                name: "private_email",
                table: "organization");

            migrationBuilder.DropColumn(
                name: "state",
                table: "organization");

            migrationBuilder.RenameColumn(
                name: "public_email",
                table: "organization",
                newName: "email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email",
                table: "organization",
                newName: "public_email");

            migrationBuilder.AddColumn<string>(
                name: "alias",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "date_status",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "level",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "default_authorization",
                table: "organization",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "localization",
                table: "organization",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "private_email",
                table: "organization",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "organization",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
