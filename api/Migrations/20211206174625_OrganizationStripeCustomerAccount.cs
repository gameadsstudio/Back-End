using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class OrganizationStripeCustomerAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "money",
                table: "organization",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "stripe_account",
                table: "organization",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "money",
                table: "organization");

            migrationBuilder.DropColumn(
                name: "stripe_account",
                table: "organization");
        }
    }
}
