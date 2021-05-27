using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationsMySql.Migrations
{
    public partial class Mig001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Procedures_Name",
                table: "Procedures",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_DateTime_DoctorId_ClientId",
                table: "Bookings",
                columns: new[] { "DateTime", "DoctorId", "ClientId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Procedures_Name",
                table: "Procedures");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_DateTime_DoctorId_ClientId",
                table: "Bookings");
        }
    }
}
