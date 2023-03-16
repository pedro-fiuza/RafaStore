using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RafaStore.Server.Migrations
{
    public partial class passwordmngm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "User",
                type: "NVARCHAR",
                nullable: false,
                defaultValue: "");
        }
    }
}
