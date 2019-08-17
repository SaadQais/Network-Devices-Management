using Microsoft.EntityFrameworkCore.Migrations;

namespace NetworksManagement.Data.Migrations
{
    public partial class AddIsMonitoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMonitoring",
                table: "Devices",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMonitoring",
                table: "Devices");
        }
    }
}
