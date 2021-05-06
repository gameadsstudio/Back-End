using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AdContainerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ad_container",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    aspect_ratio = table.Column<string>(type: "text", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    depth = table.Column<int>(type: "integer", nullable: false),
                    date_creation = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_update = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ad_container", x => x.id);
                    table.ForeignKey(
                        name: "fk_ad_container_organization_org_id",
                        column: x => x.org_id,
                        principalTable: "organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ad_container_model_tag_model",
                columns: table => new
                {
                    ad_containers_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tags_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ad_container_model_tag_model", x => new { x.ad_containers_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_ad_container_model_tag_model_ad_container_ad_containers_id",
                        column: x => x.ad_containers_id,
                        principalTable: "ad_container",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ad_container_model_tag_model_tag_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tag",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_ad_container_org_id",
                table: "ad_container",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_ad_container_model_tag_model_tags_id",
                table: "ad_container_model_tag_model",
                column: "tags_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ad_container_model_tag_model");

            migrationBuilder.DropTable(
                name: "ad_container");
        }
    }
}
