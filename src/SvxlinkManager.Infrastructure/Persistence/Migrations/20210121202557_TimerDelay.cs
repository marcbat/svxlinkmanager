using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Infrastructure.Persistence
{
    public partial class TimerDelay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c63e57a6-5652-4a43-8580-d8ac289495b8");

            migrationBuilder.AddColumn<int>(
                name: "TimerDelay",
                table: "Channels",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "66356e37-1a92-4890-a44a-9422ff48b3fe", "17202d1b-dfaf-4e48-a872-916b2ae1606a", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 1,
                column: "TimerDelay",
                value: 180);

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 2,
                column: "TimerDelay",
                value: 180);

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 3,
                column: "TimerDelay",
                value: 180);

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 4,
                column: "TimerDelay",
                value: 180);

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 5,
                column: "TimerDelay",
                value: 180);

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 6,
                column: "TimerDelay",
                value: 180);

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 7,
                column: "TimerDelay",
                value: 180);

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 8,
                column: "TimerDelay",
                value: 180);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "66356e37-1a92-4890-a44a-9422ff48b3fe");

            migrationBuilder.DropColumn(
                name: "TimerDelay",
                table: "Channels");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c63e57a6-5652-4a43-8580-d8ac289495b8", "34e6948f-ad81-418d-a862-47c01aae7ac1", "Admin", "ADMIN" });
        }
    }
}