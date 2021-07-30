using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class refactoAds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_begin",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "date_end",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "status",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "type",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "status",
                table: "advertisement");

            migrationBuilder.AddColumn<Guid>(
                name: "campaign_id",
                table: "advertisement",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "advertisement",
                type: "text",
                nullable: true);

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
                name: "ix_advertisement_campaign_id",
                table: "advertisement",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "ix_advertisement_model_tag_model_tags_id",
                table: "advertisement_model_tag_model",
                column: "tags_id");

            migrationBuilder.AddForeignKey(
                name: "fk_advertisement_campaign_campaign_id",
                table: "advertisement",
                column: "campaign_id",
                principalTable: "campaign",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_advertisement_campaign_campaign_id",
                table: "advertisement");

            migrationBuilder.DropTable(
                name: "advertisement_model_tag_model");

            migrationBuilder.DropIndex(
                name: "ix_advertisement_campaign_id",
                table: "advertisement");

            migrationBuilder.DropColumn(
                name: "campaign_id",
                table: "advertisement");

            migrationBuilder.DropColumn(
                name: "name",
                table: "advertisement");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "date_begin",
                table: "campaign",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "date_end",
                table: "campaign",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "campaign",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "campaign",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "advertisement",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
