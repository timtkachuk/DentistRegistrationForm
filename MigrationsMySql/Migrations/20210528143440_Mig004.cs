using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationsMySql.Migrations
{
    public partial class Mig004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Procedures_ProcedureId",
                table: "Bookings");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Procedures_ProcedureId",
                table: "Bookings",
                column: "ProcedureId",
                principalTable: "Procedures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Procedures_ProcedureId",
                table: "Bookings");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Procedures_ProcedureId",
                table: "Bookings",
                column: "ProcedureId",
                principalTable: "Procedures",
                principalColumn: "Id");
        }
    }
}
