using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class TagModelMediaModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tag_media_media_model_id",
                table: "tag");

            migrationBuilder.DropIndex(
                name: "ix_tag_media_model_id",
                table: "tag");

            migrationBuilder.DropColumn(
                name: "media_model_id",
                table: "tag");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "user",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "media_model_tag_model",
                columns: table => new
                {
                    medias_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tags_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_model_tag_model", x => new { x.medias_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_media_model_tag_model_media_medias_id",
                        column: x => x.medias_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_media_model_tag_model_tag_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tag",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_media_model_tag_model_tags_id",
                table: "media_model_tag_model",
                column: "tags_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "media_model_tag_model");

            migrationBuilder.AlterColumn<int>(
                name: "type",
                table: "user",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "media_model_id",
                table: "tag",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_tag_media_model_id",
                table: "tag",
                column: "media_model_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tag_media_media_model_id",
                table: "tag",
                column: "media_model_id",
                principalTable: "media",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
