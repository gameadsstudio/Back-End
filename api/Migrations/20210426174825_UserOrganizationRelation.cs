using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class UserOrganizationRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
