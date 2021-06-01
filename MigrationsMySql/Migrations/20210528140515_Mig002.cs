using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationsMySql.Migrations
{
    public partial class Mig002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingState",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingState",
                table: "Bookings");
        }
    }
}
