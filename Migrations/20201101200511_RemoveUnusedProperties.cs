using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetingPlanner.Migrations
{
    public partial class RemoveUnusedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recurring",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "WithTime",
                table: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Recurring",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WithTime",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
