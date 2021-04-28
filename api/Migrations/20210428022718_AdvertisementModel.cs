using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AdvertisementModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "advertisement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    age_min = table.Column<int>(type: "integer", nullable: false),
                    age_max = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: true),
                    date_creation = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_update = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_advertisement", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "advertisement");
        }
    }
}
