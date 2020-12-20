using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
    public partial class SquelchDetection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f227fe16-2558-4028-9c7d-9f3c44807714");

            migrationBuilder.AlterColumn<string>(
                name: "Volume",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TxCtcss",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Squelch",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RxCtcss",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PreEmph",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LowPass",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HightPass",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SquelchDetection",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ec395f1f-0493-4f42-bf0d-5f2edd139292", "4d9427b6-65dd-44ac-8ebe-801a332ae0a9", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDefault",
                value: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec395f1f-0493-4f42-bf0d-5f2edd139292");

            migrationBuilder.DropColumn(
                name: "SquelchDetection",
                table: "RadioProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "Volume",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "TxCtcss",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Squelch",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "RxCtcss",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "PreEmph",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "LowPass",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "HightPass",
                table: "RadioProfiles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f227fe16-2558-4028-9c7d-9f3c44807714", "0a771380-5e3c-460b-aac3-f499dc48cf1b", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDefault",
                value: true);
        }
    }
}
