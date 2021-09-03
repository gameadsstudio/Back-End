using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class Medias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "media_model_id",
                table: "tag",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "media_id",
                table: "advertisement",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    state_message = table.Column<string>(type: "text", nullable: true),
                    date_creation = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_update = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media", x => x.id);
                    table.ForeignKey(
                        name: "fk_media_organization_org_id",
                        column: x => x.org_id,
                        principalTable: "organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "media_2D",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_id = table.Column<Guid>(type: "uuid", nullable: false),
                    aspect_ratio = table.Column<string>(type: "text", nullable: false),
                    texture_link = table.Column<string>(type: "text", nullable: true),
                    normal_map_link = table.Column<string>(type: "text", nullable: true),
                    date_creation = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_update = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_2d", x => x.id);
                    table.ForeignKey(
                        name: "fk_media_2d_media_media_id",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "media_3D",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_id = table.Column<Guid>(type: "uuid", nullable: false),
                    model_link = table.Column<string>(type: "text", nullable: true),
                    texture_link = table.Column<string>(type: "text", nullable: true),
                    normal_map_link = table.Column<string>(type: "text", nullable: true),
                    width = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    depth = table.Column<int>(type: "integer", nullable: false),
                    date_creation = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_update = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_3d", x => x.id);
                    table.ForeignKey(
                        name: "fk_media_3d_media_media_id",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "media_unity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_bundle_link = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<int>(type: "integer", nullable: false),
                    state_message = table.Column<string>(type: "text", nullable: true),
                    date_creation = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_update = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_unity", x => x.id);
                    table.ForeignKey(
                        name: "fk_media_unity_media_media_id",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tag_media_model_id",
                table: "tag",
                column: "media_model_id");

            migrationBuilder.CreateIndex(
                name: "ix_advertisement_media_id",
                table: "advertisement",
                column: "media_id");

            migrationBuilder.CreateIndex(
                name: "ix_media_org_id",
                table: "media",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_media_2d_media_id",
                table: "media_2D",
                column: "media_id");

            migrationBuilder.CreateIndex(
                name: "ix_media_3d_media_id",
                table: "media_3D",
                column: "media_id");

            migrationBuilder.CreateIndex(
                name: "ix_media_unity_media_id",
                table: "media_unity",
                column: "media_id");

            migrationBuilder.AddForeignKey(
                name: "fk_advertisement_media_media_id",
                table: "advertisement",
                column: "media_id",
                principalTable: "media",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_tag_media_media_model_id",
                table: "tag",
                column: "media_model_id",
                principalTable: "media",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_advertisement_media_media_id",
                table: "advertisement");

            migrationBuilder.DropForeignKey(
                name: "fk_tag_media_media_model_id",
                table: "tag");

            migrationBuilder.DropTable(
                name: "media_2D");

            migrationBuilder.DropTable(
                name: "media_3D");

            migrationBuilder.DropTable(
                name: "media_unity");

            migrationBuilder.DropTable(
                name: "media");

            migrationBuilder.DropIndex(
                name: "ix_tag_media_model_id",
                table: "tag");

            migrationBuilder.DropIndex(
                name: "ix_advertisement_media_id",
                table: "advertisement");

            migrationBuilder.DropColumn(
                name: "media_model_id",
                table: "tag");

            migrationBuilder.DropColumn(
                name: "media_id",
                table: "advertisement");
        }
    }
}
