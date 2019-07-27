using Microsoft.EntityFrameworkCore.Migrations;

namespace NetworksManagement.Data.Migrations
{
    public partial class ModifyDeviceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceModels_Categories_DeviceId",
                table: "DeviceModels");

            migrationBuilder.DropIndex(
                name: "IX_DeviceModels_DeviceId",
                table: "DeviceModels");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "DeviceModels");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_CategoryId",
                table: "DeviceModels",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceModels_Categories_CategoryId",
                table: "DeviceModels",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceModels_Categories_CategoryId",
                table: "DeviceModels");

            migrationBuilder.DropIndex(
                name: "IX_DeviceModels_CategoryId",
                table: "DeviceModels");

            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "DeviceModels",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_DeviceId",
                table: "DeviceModels",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceModels_Categories_DeviceId",
                table: "DeviceModels",
                column: "DeviceId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
