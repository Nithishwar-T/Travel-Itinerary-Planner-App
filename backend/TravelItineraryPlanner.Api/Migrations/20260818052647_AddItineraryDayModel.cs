using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelItineraryPlanner.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddItineraryDayModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ItineraryDays",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ItineraryDays");
        }
    }
}
