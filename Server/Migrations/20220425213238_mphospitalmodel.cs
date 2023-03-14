using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RafaStore.Server.Migrations
{
    public partial class mphospitalmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Hospital",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_HospitalId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "Event");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HospitalId",
                table: "Event",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Event_HospitalId",
                table: "Event",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Hospital",
                table: "Event",
                column: "HospitalId",
                principalTable: "Hospital",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
