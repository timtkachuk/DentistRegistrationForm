using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationsMySql.Migrations
{
    public partial class Mig005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_DateTime_DoctorId_ClientId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_DateTime_DoctorId_ProcedureId",
                table: "Bookings",
                columns: new[] { "DateTime", "DoctorId", "ProcedureId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_DateTime_DoctorId_ProcedureId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_DateTime_DoctorId_ClientId",
                table: "Bookings",
                columns: new[] { "DateTime", "DoctorId", "ClientId" });
        }
    }
}
