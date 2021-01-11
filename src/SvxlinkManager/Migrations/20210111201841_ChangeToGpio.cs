using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
    public partial class ChangeToGpio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec395f1f-0493-4f42-bf0d-5f2edd139292");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ef085a29-8f86-482d-9621-65032bd88e03", "0c1cf7a0-6d0d-4b88-9a76-22fa55aa9ae2", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "RadioProfiles",
                keyColumn: "Id",
                keyValue: 1,
                column: "SquelchDetection",
                value: "GPIO");

            migrationBuilder.UpdateData(
                table: "RadioProfiles",
                keyColumn: "Id",
                keyValue: 2,
                column: "SquelchDetection",
                value: "GPIO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef085a29-8f86-482d-9621-65032bd88e03");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ec395f1f-0493-4f42-bf0d-5f2edd139292", "4d9427b6-65dd-44ac-8ebe-801a332ae0a9", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "RadioProfiles",
                keyColumn: "Id",
                keyValue: 1,
                column: "SquelchDetection",
                value: "CTCSS");

            migrationBuilder.UpdateData(
                table: "RadioProfiles",
                keyColumn: "Id",
                keyValue: 2,
                column: "SquelchDetection",
                value: "CTCSS");
        }
    }
}
