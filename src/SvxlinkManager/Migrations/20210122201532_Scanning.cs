using Microsoft.EntityFrameworkCore.Migrations;

namespace SvxlinkManager.Migrations
{
    public partial class Scanning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "66356e37-1a92-4890-a44a-9422ff48b3fe");

            migrationBuilder.AddColumn<string>(
                name: "TrackerUrl",
                table: "Channels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScanProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ScanDelay = table.Column<int>(type: "INTEGER", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChannelScanProfile",
                columns: table => new
                {
                    ChannelsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScanProfilesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelScanProfile", x => new { x.ChannelsId, x.ScanProfilesId });
                    table.ForeignKey(
                        name: "FK_ChannelScanProfile_Channels_ChannelsId",
                        column: x => x.ChannelsId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelScanProfile_ScanProfile_ScanProfilesId",
                        column: x => x.ScanProfilesId,
                        principalTable: "ScanProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5a751d75-1137-415c-add4-ff5f9b88003a", "b0391070-934b-4215-a6b2-00be311608e9", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 1,
                column: "TrackerUrl",
                value: "http://rrf.f5nlg.ovh:8080/RRFTracker/RRF-today/rrf_tiny.json");

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 3,
                column: "TrackerUrl",
                value: "http://rrf.f5nlg.ovh:8080/RRFTracker/FON-today/rrf_tiny.json");

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 4,
                column: "TrackerUrl",
                value: "http://rrf.f5nlg.ovh:8080/RRFTracker/TECHNIQUE-today/rrf_tiny.json");

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 5,
                column: "TrackerUrl",
                value: "http://rrf.f5nlg.ovh:8080/RRFTracker/INTERNATIONAL-today/rrf_tiny.json");

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 6,
                column: "TrackerUrl",
                value: "http://rrf.f5nlg.ovh:8080/RRFTracker/BAVARDAGE-today/rrf_tiny.json");

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 7,
                column: "TrackerUrl",
                value: "http://rrf.f5nlg.ovh:8080/RRFTracker/LOCAL-today/rrf_tiny.json");

            migrationBuilder.UpdateData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: 8,
                column: "TrackerUrl",
                value: "");

            migrationBuilder.InsertData(
                table: "ScanProfile",
                columns: new[] { "Id", "Enable", "Name", "ScanDelay" },
                values: new object[] { 1, false, "default", 60 });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelScanProfile_ScanProfilesId",
                table: "ChannelScanProfile",
                column: "ScanProfilesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelScanProfile");

            migrationBuilder.DropTable(
                name: "ScanProfile");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a751d75-1137-415c-add4-ff5f9b88003a");

            migrationBuilder.DropColumn(
                name: "TrackerUrl",
                table: "Channels");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "66356e37-1a92-4890-a44a-9422ff48b3fe", "17202d1b-dfaf-4e48-a872-916b2ae1606a", "Admin", "ADMIN" });
        }
    }
}
