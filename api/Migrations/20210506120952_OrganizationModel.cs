using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class OrganizationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "modification_date",
                table: "organization",
                newName: "date_update");

            migrationBuilder.RenameColumn(
                name: "creation_date",
                table: "organization",
                newName: "date_creation");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "organization",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "state",
                table: "organization",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "default_authorization",
                table: "organization",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "organization_model_user_model",
                columns: table => new
                {
                    organizations_id = table.Column<Guid>(type: "uuid", nullable: false),
                    users_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organization_model_user_model", x => new { x.organizations_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_organization_model_user_model_organization_organizations_id",
                        column: x => x.organizations_id,
                        principalTable: "organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organization_model_user_model_user_users_id",
                        column: x => x.users_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_organization_model_user_model_users_id",
                table: "organization_model_user_model",
                column: "users_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "organization_model_user_model");

            migrationBuilder.RenameColumn(
                name: "date_update",
                table: "organization",
                newName: "modification_date");

            migrationBuilder.RenameColumn(
                name: "date_creation",
                table: "organization",
                newName: "creation_date");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "organization",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "state",
                table: "organization",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "default_authorization",
                table: "organization",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
