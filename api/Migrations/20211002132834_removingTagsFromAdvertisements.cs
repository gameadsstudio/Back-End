using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class removingTagsFromAdvertisements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "advertisement_model_tag_model");

            migrationBuilder.AlterColumn<int>(
                name: "age_min",
                table: "campaign",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "age_max",
                table: "campaign",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "tag_model_id",
                table: "advertisement",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_advertisement_tag_model_id",
                table: "advertisement",
                column: "tag_model_id");

            migrationBuilder.AddForeignKey(
                name: "fk_advertisement_tag_tag_model_id",
                table: "advertisement",
                column: "tag_model_id",
                principalTable: "tag",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_advertisement_tag_tag_model_id",
                table: "advertisement");

            migrationBuilder.DropIndex(
                name: "ix_advertisement_tag_model_id",
                table: "advertisement");

            migrationBuilder.DropColumn(
                name: "tag_model_id",
                table: "advertisement");

            migrationBuilder.AlterColumn<string>(
                name: "age_min",
                table: "campaign",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "age_max",
                table: "campaign",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "advertisement_model_tag_model",
                columns: table => new
                {
                    advertisements_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tags_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_advertisement_model_tag_model", x => new { x.advertisements_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_advertisement_model_tag_model_advertisement_advertisements_",
                        column: x => x.advertisements_id,
                        principalTable: "advertisement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_advertisement_model_tag_model_tag_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tag",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_advertisement_model_tag_model_tags_id",
                table: "advertisement_model_tag_model",
                column: "tags_id");
        }
    }
}
