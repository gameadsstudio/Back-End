using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class ChangeUserOrgaRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_organization_org_id",
                table: "user");

            migrationBuilder.DropIndex(
                name: "ix_user_org_id",
                table: "user");

            migrationBuilder.DropColumn(
                name: "org_id",
                table: "user");

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

            migrationBuilder.AddColumn<Guid>(
                name: "org_id",
                table: "user",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_org_id",
                table: "user",
                column: "org_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_organization_org_id",
                table: "user",
                column: "org_id",
                principalTable: "organization",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
