using Microsoft.EntityFrameworkCore.Migrations;

namespace NetworksManagement.Data.Migrations
{
    public partial class AddVersionToDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Devices");
        }
    }
}
