using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "organization",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    public_email = table.Column<string>(type: "text", nullable: true),
                    private_email = table.Column<string>(type: "text", nullable: false),
                    localization = table.Column<string>(type: "text", nullable: true),
                    logo_url = table.Column<string>(type: "text", nullable: true),
                    website_url = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    default_authorization = table.Column<string>(type: "text", nullable: true),
                    creation_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modification_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organization", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    firstname = table.Column<string>(type: "text", nullable: true),
                    lastname = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    level = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    date_status = table.Column<string>(type: "text", nullable: true),
                    date_creation = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_update = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    age_min = table.Column<string>(type: "text", nullable: true),
                    age_max = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    date_begin = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_creation = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_update = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campaign", x => x.id);
                    table.ForeignKey(
                        name: "fk_campaign_organization_org_id",
                        column: x => x.org_id,
                        principalTable: "organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    date_launch = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_creation = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_update = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game", x => x.id);
                    table.ForeignKey(
                        name: "fk_game_organization_org_id",
                        column: x => x.org_id,
                        principalTable: "organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_campaign_org_id",
                table: "campaign",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_org_id",
                table: "game",
                column: "org_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "campaign");

            migrationBuilder.DropTable(
                name: "game");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "organization");
        }
    }
}
