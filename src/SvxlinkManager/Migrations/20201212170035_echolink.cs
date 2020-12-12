using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
    public partial class echolink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32682d52-b892-4877-aed2-a8cd42077960");

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.AlterColumn<string>(
                name: "ReportCallSign",
                table: "Channels",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Port",
                table: "Channels",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Channels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Channels",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Channels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxQso",
                table: "Channels",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Channels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SysopName",
                table: "Channels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f227fe16-2558-4028-9c7d-9f3c44807714", "0a771380-5e3c-460b-aac3-f499dc48cf1b", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Discriminator", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 2, "xD9wW5gO7yD9hN5o", "(CH) SVX4LINK H", "SvxlinkChannel", 104, "salonsuisseromand.northeurope.cloudapp.azure.com", false, true, "Salon Suisse Romand", 5300, "SVX4LINK", "Sreg.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Discriminator", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 1, "Magnifique123456789!", "(CH) SVX4LINK H", "SvxlinkChannel", 96, "rrf2.f5nlg.ovh", true, false, "Réseau des Répéteurs Francophones", 5300, "SVX4LINK", "Srrf.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Discriminator", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 8, "Magnifique123456789!", "(CH) SVX4LINK H", "SvxlinkChannel", 102, "rrf3.f5nlg.ovh", false, true, "Salon Expérimental", 5303, "SVX4LINK", "Sexp.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Discriminator", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 7, "FON-F1TZO", "(CH) SVX4LINK H", "SvxlinkChannel", 101, "serveur.f1tzo.com", false, true, "Salon Local", 5302, "SVX4LINK", "Sloc.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Discriminator", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 6, "FON-F1TZO", "(CH) SVX4LINK H", "SvxlinkChannel", 100, "serveur.f1tzo.com", false, true, "Salon Bavardage", 5301, "SVX4LINK", "Sbav.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Discriminator", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 5, "Magnifique123456789!", "(CH) SVX4LINK H", "SvxlinkChannel", 99, "rrf3.f5nlg.ovh", false, true, "Salon International", 5302, "SVX4LINK", "Sint.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Discriminator", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 4, "Magnifique123456789!", "(CH) SVX4LINK H", "SvxlinkChannel", 98, "rrf3.f5nlg.ovh", false, true, "Salon Technique", 5301, "SVX4LINK", "Stec.wav" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "AuthKey", "CallSign", "Discriminator", "Dtmf", "Host", "IsDefault", "IsTemporized", "Name", "Port", "ReportCallSign", "SoundName" },
                values: new object[] { 3, "FON-F1TZO", "(CH) SVX4LINK H", "SvxlinkChannel", 97, "serveur.f1tzo.com", false, true, "French Open Network", 5300, "SVX4LINK", "Sfon.wav" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f227fe16-2558-4028-9c7d-9f3c44807714");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "MaxQso",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "SysopName",
                table: "Channels");

            migrationBuilder.AlterColumn<string>(
                name: "ReportCallSign",
                table: "Channels",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Port",
                table: "Channels",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "32682d52-b892-4877-aed2-a8cd42077960", "9cae3b52-c005-4422-90ed-157b5837ebfb", "Admin", "ADMIN" });
        }
    }
}
