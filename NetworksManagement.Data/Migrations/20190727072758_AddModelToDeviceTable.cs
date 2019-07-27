using Microsoft.EntityFrameworkCore.Migrations;

namespace NetworksManagement.Data.Migrations
{
    public partial class AddModelToDeviceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModelId",
                table: "Devices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ModelId",
                table: "Devices",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_DeviceModels_ModelId",
                table: "Devices",
                column: "ModelId",
                principalTable: "DeviceModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceModels_ModelId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_ModelId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Devices");
        }
    }
}
