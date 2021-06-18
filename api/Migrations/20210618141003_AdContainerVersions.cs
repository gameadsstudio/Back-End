using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AdContainerVersions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_ad_container_organization_org_id",
                table: "ad_container");

            migrationBuilder.AlterColumn<Guid>(
                name: "org_id",
                table: "ad_container",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "version_id",
                table: "ad_container",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_ad_container_version_id",
                table: "ad_container",
                column: "version_id");

            migrationBuilder.AddForeignKey(
                name: "fk_ad_container_organization_org_id",
                table: "ad_container",
                column: "org_id",
                principalTable: "organization",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_ad_container_version_version_id",
                table: "ad_container",
                column: "version_id",
                principalTable: "version",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_ad_container_organization_org_id",
                table: "ad_container");

            migrationBuilder.DropForeignKey(
                name: "fk_ad_container_version_version_id",
                table: "ad_container");

            migrationBuilder.DropIndex(
                name: "ix_ad_container_version_id",
                table: "ad_container");

            migrationBuilder.DropColumn(
                name: "version_id",
                table: "ad_container");

            migrationBuilder.AlterColumn<Guid>(
                name: "org_id",
                table: "ad_container",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_ad_container_organization_org_id",
                table: "ad_container",
                column: "org_id",
                principalTable: "organization",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
