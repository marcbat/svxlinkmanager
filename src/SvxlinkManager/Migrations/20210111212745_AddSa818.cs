using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
    public partial class AddSa818 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef085a29-8f86-482d-9621-65032bd88e03");

            migrationBuilder.AddColumn<bool>(
                name: "HasSa818",
                table: "RadioProfiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c63e57a6-5652-4a43-8580-d8ac289495b8", "34e6948f-ad81-418d-a862-47c01aae7ac1", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "RadioProfiles",
                keyColumn: "Id",
                keyValue: 1,
                column: "HasSa818",
                value: true);

            migrationBuilder.UpdateData(
                table: "RadioProfiles",
                keyColumn: "Id",
                keyValue: 2,
                column: "HasSa818",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c63e57a6-5652-4a43-8580-d8ac289495b8");

            migrationBuilder.DropColumn(
                name: "HasSa818",
                table: "RadioProfiles");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ef085a29-8f86-482d-9621-65032bd88e03", "0c1cf7a0-6d0d-4b88-9a76-22fa55aa9ae2", "Admin", "ADMIN" });
        }
    }
}
